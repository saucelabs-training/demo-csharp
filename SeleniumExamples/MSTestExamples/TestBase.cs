using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace MSTest;

public class TestBase
{
    protected IWebDriver driver;
    private string sessionId;
    private string testName;
    private static readonly string BuildName;
    private static readonly string BuildIdentifier;

    static TestBase()
    {
        BuildName = "DotNet MSTest Examples";
        string buildNumber = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
        BuildIdentifier = $"{BuildName}: {buildNumber}";
    }
    
    protected void StartChromeSession(TestContext context)
    {
        testName = context.TestName;

        var options = new ChromeOptions();
        options.AddUserProfilePreference("profile.password_manager_leak_detection", false);

        options.AddAdditionalOption("sauce:options", DefaultSauceOptions(context));

        if (string.IsNullOrEmpty(options.PlatformName))
        {
            options.PlatformName = "Windows 11";
        }

        driver = new RemoteWebDriver(new Uri("https://ondemand.us-west-1.saucelabs.com/wd/hub"), options);
        sessionId = ((RemoteWebDriver)driver).SessionId.ToString();
    }

    private static Dictionary<string, object> DefaultSauceOptions(TestContext context)
    {
        return new Dictionary<string, object>
        {
            ["username"] = Environment.GetEnvironmentVariable("SAUCE_USERNAME"),
            ["accessKey"] = Environment.GetEnvironmentVariable("SAUCE_ACCESS_KEY"),
            ["name"] = context.TestName,
            ["build"] = BuildIdentifier,
            ["seleniumVersion"] = "4.33.0"
        };
    }

    protected void QuitSession(TestContext context)
    {
        try
        {
            string result = context.CurrentTestOutcome == UnitTestOutcome.Passed ? "passed" : "failed";
            ((IJavaScriptExecutor)driver).ExecuteScript($"sauce:job-result={result}");
        }
        catch (Exception e)
        {
            Console.WriteLine("Problem updating Sauce job result: " + e);
        }
        finally
        {
            PrintResults();
            driver?.Quit();
        }
    }

    private void PrintResults()
    {
        Console.WriteLine($"SauceOnDemandSessionID={sessionId} job-name={testName}");
        Console.WriteLine($"Test Job Link: https://app.saucelabs.com/tests/{sessionId}");
    }
}