using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;

namespace Core.BestPractices.Web.Tests
{
    [TestFixture]
    public class AllTestsBase
    {
        public IWebDriver Driver { get; set; }

        public static string SauceUserName => Environment.GetEnvironmentVariable("SAUCE_USERNAME", EnvironmentVariableTarget.User);
        public static string SauceAccessKey => Environment.GetEnvironmentVariable("SAUCE_ACCESS_KEY", EnvironmentVariableTarget.User);
        public Dictionary<string, object> SauceOptions;
        public static string ScreenerApiKey => Environment.GetEnvironmentVariable("SCREENER_API_KEY", EnvironmentVariableTarget.User);
        public IJavaScriptExecutor JsExecutor => (IJavaScriptExecutor)Driver;

        public IWebDriver GetVisualDriver(ICapabilities capabilities)
        {
            //TimeSpan.FromSeconds(120) = needed so that there isn't a 'The HTTP request to the remote WebDriver server for URL' error
            var driver = new RemoteWebDriver(new Uri("https://hub.screener.io:443/wd/hub"), capabilities,
                TimeSpan.FromSeconds(120));
            //Needed so that Screener 'end' command doesn't timeout
            driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(60);
            return driver;
        }
        public IWebDriver GetDesktopDriver(ICapabilities browserOptions)
        {
            return new RemoteWebDriver(new Uri("https://ondemand.saucelabs.com/wd/hub"), browserOptions);
        }

        public void ExecuteSauceCleanupSteps(IWebDriver driver)
        {
            var isPassed = TestContext.CurrentContext.Result.Outcome.Status
                           == TestStatus.Passed;
            var script = "sauce:job-result=" + (isPassed ? "passed" : "failed");
            ((IJavaScriptExecutor)driver).ExecuteScript(script);
        }
    }
}