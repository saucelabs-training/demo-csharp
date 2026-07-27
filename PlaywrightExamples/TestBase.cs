using System.Collections.Concurrent;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;
using Microsoft.Playwright;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace PlaywrightExamples;

public static class BuildInfo
{
    public static readonly string Name = "DotNet Playwright Examples";
    public static readonly string Identifier = $"{Name}: {DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";
}

// A session (local browser or Sauce Labs job) is checked out per test and, on teardown, either
// kept alive for reuse (test passed) or closed immediately (test failed) so nobody ever inherits a
// possibly-dirty browser. GROUPING decides how sessions are shared:
// - "class" (default): one session per test class/fixture, named after the class. Relies on the
//   test classes being [Parallelizable(ParallelScope.Fixtures)] - fixtures run in parallel with
//   each other, but tests *within* one fixture run sequentially, so exactly one test at a time
//   ever touches a given class's session (no locking needed, and a mid-class failure can safely
//   close the shared browser without yanking it out from under a concurrently running sibling).
// - "test": one dedicated, never-shared session per test, named after the test itself.
// TARGET selects whether that session is a locally launched browser or a Sauce Labs one - BROWSER
// (chromium/firefox/webkit) means the same thing either way, so switching targets is a one-line change.
[TestFixture]
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public abstract class TestBase
{
    private enum Target
    {
        Local,
        Sauce
    }

    private enum SessionGrouping
    {
        Class,
        Test
    }

    private static readonly Target RunTarget =
        string.Equals(Environment.GetEnvironmentVariable("TARGET"), "sauce", StringComparison.OrdinalIgnoreCase)
            ? Target.Sauce
            : Target.Local;

    private static readonly SessionGrouping Grouping =
        string.Equals(Environment.GetEnvironmentVariable("GROUPING"), "test", StringComparison.OrdinalIgnoreCase)
            ? SessionGrouping.Test
            : SessionGrouping.Class;

    private static readonly string BrowserName = Environment.GetEnvironmentVariable("BROWSER") ?? "chromium";

    private static readonly string SauceUsername = Environment.GetEnvironmentVariable("SAUCE_USERNAME") ?? "";
    private static readonly string SauceAccessKey = Environment.GetEnvironmentVariable("SAUCE_ACCESS_KEY") ?? "";
    private static readonly string SauceRegion = Environment.GetEnvironmentVariable("SAUCE_REGION") ?? "us-west-1";
    private static readonly string SauceUrl = $"https://ondemand.{SauceRegion}.saucelabs.com";
    private static readonly string SauceApiUrl = $"https://api.{SauceRegion}.saucelabs.com";

    // The native session endpoint only accepts a major.minor version (e.g. "1.58"), not the full package version.
    private static readonly string PlaywrightVersion = GetPlaywrightVersion();

    // "class" mode only: one entry per test class, holding that class's shared session. Lazy<Task<>>
    // means only one caller ever actually opens the session even if this assumption is ever violated.
    private static readonly ConcurrentDictionary<string, Lazy<Task<WorkerSession>>> ClassSessions = new();

    protected IPage Page { get; private set; } = null!;

    private WorkerSession _session = null!;
    private IBrowserContext? _context;

    [SetUp]
    public async Task BaseSetUp()
    {
        if (Grouping == SessionGrouping.Class)
        {
            var className = ClassSessionKey();
            var lazySession = ClassSessions.GetOrAdd(className, _ =>
                new Lazy<Task<WorkerSession>>(() => OpenSessionAsync(ShortClassName(className)),
                    LazyThreadSafetyMode.ExecutionAndPublication));
            _session = await lazySession.Value;
        }
        else
        {
            _session = await OpenSessionAsync(TestContext.CurrentContext.Test.Name);
        }

        _context = await _session.Browser.NewContextAsync();
        Page = await _context.NewPageAsync();
    }

    // The same key must be used everywhere ClassSessions is read or written (BaseSetUp and the
    // eviction path in BaseTearDown) - a mismatch would let two different fixtures collide on one
    // entry, or let teardown fail to find the entry it just used. ClassName is only ever null for
    // a test with no fixture, which GROUPING=class doesn't support - fail loudly rather than
    // silently bucket such a test under a shared "" key.
    private static string ClassSessionKey() =>
        TestContext.CurrentContext.Test.ClassName
        ?? throw new InvalidOperationException("GROUPING=class requires the test to belong to a fixture (ClassName was null).");

    private static string ShortClassName(string fullClassName) => fullClassName.Split('.')[^1];

    private static async Task<WorkerSession> OpenSessionAsync(string sessionName)
    {
        var playwright = await Playwright.CreateAsync();
        var browserType = GetBrowserType(playwright, BrowserName);

        if (RunTarget == Target.Local)
        {
            var browser = await browserType.LaunchAsync();
            return new WorkerSession(playwright, browser);
        }

        var requestContext = await playwright.APIRequest.NewContextAsync(new APIRequestNewContextOptions
        {
            BaseURL = SauceUrl,
            HttpCredentials = new HttpCredentials
            {
                Username = SauceUsername,
                Password = SauceAccessKey,
                Send = HttpCredentialsSend.Always
            }
        });

        var payload = BuildCapabilitiesPayload(sessionName, BrowserName);

        // The endpoint 303-redirects while the VM spins up; follow until we get a 200.
        var response = await requestContext.PostAsync("playwright/session", new APIRequestContextOptions
        {
            DataObject = payload,
            MaxRedirects = 0,
            Timeout = 120000
        });

        while (response.Status == 303)
        {
            var location = response.Headers["location"];
            response = await requestContext.GetAsync(location, new APIRequestContextOptions { MaxRedirects = 0 });
        }

        var responseText = await response.TextAsync();
        using var jsonDoc = JsonDocument.Parse(responseText);
        var root = jsonDoc.RootElement;
        var value = root.TryGetProperty("value", out var v) ? v : root;

        var sessionId = value.GetProperty("sessionId").GetString()!;
        var wsEndpoint = value.GetProperty("wsEndpoint").GetString();

        var sauceBrowser = await browserType.ConnectAsync($"{wsEndpoint}?browser={BrowserName}");

        return new WorkerSession(playwright, sauceBrowser, sessionId, requestContext);
    }

    [TearDown]
    public async Task BaseTearDown()
    {
        bool passed = false;
        try
        {
            string testName = TestContext.CurrentContext.Test.Name;
            passed = TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed;
            string result = passed ? "passed" : "failed";

            Console.WriteLine($"Test: {testName} - Result: {result}");

            if (_session != null && _session.SessionId != null)
            {
                PrintSauceJobLink(testName, _session.SessionId);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error in TearDown: {e.Message}");
        }
        finally
        {
            // Page/_session are only null if BaseSetUp threw before reaching that point - nothing
            // to clean up in that case, and touching them here would throw a NullReferenceException
            // that masks the real SetUp failure in the test result.
            if (Page != null) await Page.CloseAsync();
            if (_context != null) await _context.CloseAsync();

            if (_session != null)
            {
                if (Grouping == SessionGrouping.Class)
                {
                    // Passed: leave the session in ClassSessions for the next test in this class to
                    // reuse. Failed: evict and close it now instead of leaving a possibly-dirty
                    // browser for the next test in this class to inherit - safe to do unconditionally
                    // because ParallelScope.Fixtures guarantees no sibling test is using it right now.
                    if (!passed)
                    {
                        ClassSessions.TryRemove(ClassSessionKey(), out _);
                        await CloseSessionAsync(_session, passed: false);
                    }
                }
                else
                {
                    // "test" mode sessions are never shared, so there's nobody to hand this off to -
                    // always close and report, pass or fail.
                    await CloseSessionAsync(_session, passed);
                }
            }
        }
    }

    private static async Task CloseSessionAsync(WorkerSession session, bool passed)
    {
        try
        {
            if (session.SessionId != null && session.RequestContext != null)
            {
                await UpdateSauceResultAsync(session.RequestContext, session.SessionId, passed);
            }

            // The Sauce session ends when its WebSocket connection drops; a local browser just closes.
            await session.Browser.CloseAsync();

            if (session.RequestContext != null)
            {
                await session.RequestContext.DisposeAsync();
            }

            session.Playwright.Dispose();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error closing session {session.SessionId}: {e.Message}");
        }
    }

    // Closes every session still in ClassSessions ("test" mode never populates it - each session
    // is already closed in BaseTearDown). Invoked once after all tests finish, by
    // SauceSessionsTeardown's [OneTimeTearDown] below. Any class session that ever failed a test
    // was already evicted and closed in BaseTearDown, so everything left here only ever ran
    // passing tests - each is reported as passed.
    internal static async Task CloseAllWorkerSessionsAsync()
    {
        foreach (var (_, lazySession) in ClassSessions)
        {
            await CloseSessionAsync(await lazySession.Value, passed: true);
        }
        ClassSessions.Clear();
    }

    private static Dictionary<string, object> BuildCapabilitiesPayload(string sessionName, string browserName)
    {
        return new Dictionary<string, object>
        {
            ["browserName"] = browserName,
            ["platformName"] = "Linux",
            ["playwrightVersion"] = PlaywrightVersion,
            ["sauce:options"] = new Dictionary<string, object>
            {
                ["name"] = sessionName,
                ["build"] = BuildInfo.Identifier
            }
        };
    }

    // The session endpoint accepts 'chromium', 'firefox' or 'webkit' - the same names Playwright
    // itself uses, so no translation table is needed.
    private static IBrowserType GetBrowserType(IPlaywright playwright, string browserName) => browserName.ToLowerInvariant() switch
    {
        "chromium" => playwright.Chromium,
        "firefox" => playwright.Firefox,
        "webkit" => playwright.Webkit,
        _ => throw new ArgumentException(
            $"Unsupported BROWSER \"{browserName}\". Use one of: chromium, firefox, webkit.")
    };

    private static string GetPlaywrightVersion()
    {
        // AssemblyInformationalVersion on Microsoft.Playwright.dll is a stale "1.0.0+<hash>",
        // not the package version - AssemblyName.Version tracks the real one (e.g. 1.61.0.0).
        var version = typeof(IPage).Assembly.GetName().Version;
        return version != null ? $"{version.Major}.{version.Minor}" : "0.0";
    }

    private static async Task UpdateSauceResultAsync(IAPIRequestContext requestContext, string sessionId, bool passed)
    {
        await requestContext.PutAsync($"{SauceApiUrl}/rest/v1/{SauceUsername}/jobs/{sessionId}", new APIRequestContextOptions
        {
            Headers = new Dictionary<string, string>
            {
                ["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes($"{SauceUsername}:{SauceAccessKey}"))
            },
            DataObject = new { passed }
        });
    }

    private static void PrintSauceJobLink(string testName, string sessionId)
    {
        Console.WriteLine($"SauceOnDemandSessionID={sessionId} job-name={testName}");
        Console.WriteLine($"Test Job Link: https://app.saucelabs.com/tests/{sessionId}");
    }

    protected async Task LoginAsync(string username = "standard_user", string password = "secret_sauce")
    {
        await Page.GotoAsync("https://www.saucedemo.com/");
        await Page.Locator("[data-test='username']").FillAsync(username);
        await Page.Locator("[data-test='password']").FillAsync(password);
        await Page.Locator("[data-test='login-button']").ClickAsync();
    }

    // SessionId/RequestContext are null for a local session - there's no Sauce Labs job to report
    // results to or link back to.
    private sealed class WorkerSession(
        IPlaywright playwright,
        IBrowser browser,
        string? sessionId = null,
        IAPIRequestContext? requestContext = null)
    {
        public IPlaywright Playwright { get; } = playwright;
        public IBrowser Browser { get; } = browser;
        public string? SessionId { get; } = sessionId;
        public IAPIRequestContext? RequestContext { get; } = requestContext;
    }
}

// Runs once after every test in the assembly has finished, closing out whatever class sessions
// (local or Sauce) are still open (see ClassSessions above).
[SetUpFixture]
public class SauceSessionsTeardown
{
    [OneTimeTearDown]
    public async Task CloseAllSauceSessions()
    {
        await TestBase.CloseAllWorkerSessionsAsync();
    }
}
