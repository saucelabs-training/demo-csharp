using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace NUnitExamples;

// Immutable build info for clarity
public static class BuildInfo
{
    public static readonly string Name = "DotNet NUnit Examples Async";
    public static readonly string Identifier = $"{Name}: {DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";
}

[TestFixture]
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public abstract class TestBase
{
    protected IWebDriver Driver { get; private set; } = null!;
    private string? sessionId;
    private string? testName;

    protected virtual Task StartChromeSessionAsync()
    {
        testName = TestContext.CurrentContext.Test.Name;

        var options = new ChromeOptions();
        options.AddUserProfilePreference("profile.password_manager_leak_detection", false);

        var sauceOptions = new Dictionary<string, object?>
        {
            ["username"] = Environment.GetEnvironmentVariable("SAUCE_USERNAME"),
            ["accessKey"] = Environment.GetEnvironmentVariable("SAUCE_ACCESS_KEY"),
            ["name"] = testName,
            ["build"] = BuildInfo.Identifier,
            ["seleniumVersion"] = "4.33.0"
        };

        options.AddAdditionalOption("sauce:options", sauceOptions);
        options.PlatformName = "Windows 11";

        Driver = new RemoteWebDriver(new Uri("https://ondemand.us-west-1.saucelabs.com/wd/hub"), options);
        
        if (Driver != null)
        {
            sessionId = ((RemoteWebDriver)Driver).SessionId.ToString();
        }
        else
        {
            throw new InvalidOperationException("Failed to initialize WebDriver");
        }

        return Task.CompletedTask;
    }

    protected async Task RunWithReporting(Func<Task> testBody)
    {
        bool passed = false;

        try
        {
            await testBody().ConfigureAwait(false);
            passed = true;
        }
        finally
        {
            await QuitSessionAsync(passed).ConfigureAwait(false);
        }
    }

    protected virtual Task QuitSessionAsync(bool passed)
    {
        try
        {
            if (sessionId != null && testName != null)
            {
                string result = passed ? "passed" : "failed";
                ((IJavaScriptExecutor)Driver).ExecuteScript($"sauce:job-result={result}");

                Console.WriteLine($"SauceOnDemandSessionID={sessionId} job-name={testName}");
                Console.WriteLine($"Test Job Link: https://app.saucelabs.com/tests/{sessionId}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Problem updating Sauce job result: " + e);
        }
        finally
        {
            try
            {
                Driver.Quit();
            }
            catch (Exception e)
            {
                Console.WriteLine("Problem quitting driver: " + e);
            }
        }

        return Task.CompletedTask;
    }
}