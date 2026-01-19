using System.Text;
using System.Text.Json;
using Microsoft.Playwright;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace PlaywrightExamples;

public static class BuildInfo
{
    public static readonly string Name = "DotNet Playwright Examples";
    public static readonly string Identifier = $"{Name}: {DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";
}

[TestFixture]
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public abstract class TestBase
{
    private static readonly string SauceUsername = Environment.GetEnvironmentVariable("SAUCE_USERNAME") ?? "";
    private static readonly string SauceAccessKey = Environment.GetEnvironmentVariable("SAUCE_ACCESS_KEY") ?? "";
    private static readonly string SauceUrl = "https://ondemand.us-west-1.saucelabs.com/wd/hub/";
    
    protected IPlaywright Playwright { get; private set; } = null!;
    protected IBrowser Browser { get; private set; } = null!;
    protected IPage Page { get; private set; } = null!;
    
    private string? _sessionId;
    private IAPIRequestContext? _requestContext;

    [SetUp]
    public async Task BaseSetUp()
    {
        string testName = $"{TestContext.CurrentContext.Test.ClassName}: {TestContext.CurrentContext.Test.Name}";
        
        Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        
        // Create API request context for Sauce Labs
        _requestContext = await Playwright.APIRequest.NewContextAsync(new APIRequestNewContextOptions
        {
            BaseURL = SauceUrl
        });
        
        // Build capabilities payload
        var sauceOptions = new Dictionary<string, object>
        {
            ["username"] = SauceUsername,
            ["accessKey"] = SauceAccessKey,
            ["devTools"] = true,
            ["name"] = testName,
            ["build"] = BuildInfo.Identifier
        };
        
        var sessionRequest = new Dictionary<string, object>
        {
            ["platformName"] = "macOS 13",
            ["browserName"] = "Chrome",
            ["sauce:options"] = sauceOptions,
            ["goog:chromeOptions"] = new Dictionary<string, object>
            {
                ["args"] = new[] { "--disable-features=SafeBrowsing,PasswordLeakToggleMove" },
                ["prefs"] = new Dictionary<string, object>
                {
                    ["credentials_enable_service"] = false,
                    ["profile.password_manager_enabled"] = false,
                    ["profile.password_manager_leak_detection"] = false
                }
            }
        };
        
        var capabilities = new Dictionary<string, object>
        {
            ["alwaysMatch"] = sessionRequest
        };
        
        var payload = new Dictionary<string, object>
        {
            ["capabilities"] = capabilities
        };
        
        // Create session on Sauce Labs
        var response = await _requestContext.PostAsync("session", new APIRequestContextOptions
        {
            DataObject = payload,
            Timeout = 120000
        });
        
        var responseText = await response.TextAsync();
        using var jsonDoc = JsonDocument.Parse(responseText);
        var value = jsonDoc.RootElement.GetProperty("value");
        
        _sessionId = value.GetProperty("sessionId").GetString();
        var cdpEndpoint = value.GetProperty("capabilities").GetProperty("se:cdp").GetString();
        
        // Connect to browser via CDP
        Browser = await Playwright.Chromium.ConnectOverCDPAsync(cdpEndpoint!);
        
        // Get the default context and page, or create new ones
        var contexts = Browser.Contexts;
        if (contexts.Count > 0)
        {
            var pages = contexts[0].Pages;
            Page = pages.Count > 0 ? pages[0] : await contexts[0].NewPageAsync();
        }
        else
        {
            var context = await Browser.NewContextAsync(new BrowserNewContextOptions
            {
                ViewportSize = ViewportSize.NoViewport
            });
            Page = await context.NewPageAsync();
        }
    }

    [TearDown]
    public async Task BaseTearDown()
    {
        try
        {
            string testName = TestContext.CurrentContext.Test.Name;
            bool passed = TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed;
            string result = passed ? "passed" : "failed";

            Console.WriteLine($"Test: {testName} - Result: {result}");
            
            // Update test result on Sauce Labs
            if (_requestContext != null && !string.IsNullOrEmpty(_sessionId))
            {
                await UpdateSauceResultAsync(passed);
            }
            
            // Print Sauce Labs job link
            PrintSauceJobLink(testName);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error in TearDown: {e.Message}");
        }
        finally
        {
            if (Page != null) await Page.CloseAsync();
            if (Browser != null) await Browser.CloseAsync();
            
            // Delete session on Sauce Labs
            if (_requestContext != null && !string.IsNullOrEmpty(_sessionId))
            {
                await _requestContext.DeleteAsync($"session/{_sessionId}");
                await _requestContext.DisposeAsync();
            }
            
            Playwright?.Dispose();
        }
    }
    
    private async Task UpdateSauceResultAsync(bool passed)
    {
        using var httpClient = new HttpClient();
        var authString = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{SauceUsername}:{SauceAccessKey}"));
        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authString);
        
        var updatePayload = new { passed };
        var content = new StringContent(JsonSerializer.Serialize(updatePayload), Encoding.UTF8, "application/json");
        
        await httpClient.PutAsync($"https://api.us-west-1.saucelabs.com/rest/v1/{SauceUsername}/jobs/{_sessionId}", content);
    }
    
    private void PrintSauceJobLink(string testName)
    {
        Console.WriteLine($"SauceOnDemandSessionID={_sessionId} job-name={testName}");
        Console.WriteLine($"Test Job Link: https://app.saucelabs.com/tests/{_sessionId}");
    }
    
    protected async Task AnnotateAsync(string comment)
    {
        if (_requestContext != null && !string.IsNullOrEmpty(_sessionId))
        {
            var payload = new Dictionary<string, object>
            {
                ["script"] = $"sauce:context={comment}",
                ["args"] = Array.Empty<object>()
            };
            
            await _requestContext.PostAsync($"session/{_sessionId}/execute/sync", new APIRequestContextOptions
            {
                DataObject = payload
            });
        }
    }

    protected async Task LoginAsync(string username = "standard_user", string password = "secret_sauce")
    {
        await Page.GotoAsync("https://www.saucedemo.com/");
        await Page.Locator("[data-test='username']").FillAsync(username);
        await Page.Locator("[data-test='password']").FillAsync(password);
        await Page.Locator("[data-test='login-button']").ClickAsync();
    }
}
