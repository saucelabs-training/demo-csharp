using System;
using System.Collections.Generic;
using Core.Common;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Remote;

namespace Core.Selenium.Examples
{
    [TestFixture]
    public class AllTestsBase
    {
        public IWebDriver Driver { get; set; }

        public string SauceUserName =>
            Environment.GetEnvironmentVariable("SAUCE_USERNAME");

        public string SauceAccessKey =>
            Environment.GetEnvironmentVariable("SAUCE_ACCESS_KEY");

        public Dictionary<string, object> SauceOptions;

        public string ScreenerApiKey =>
            Environment.GetEnvironmentVariable("SCREENER_API_KEY");

        public IJavaScriptExecutor JsExecutor => (IJavaScriptExecutor) Driver;

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

        public AndroidDriver<AndroidElement> GetAndroidDriver(AppiumOptions appiumOptions)
        {
            return new(new SauceLabsEndpoint().EmusimUri(SauceUserName, SauceAccessKey), appiumOptions, TimeSpan
                .FromSeconds(240));
        }

        public IOSDriver<IOSElement> GetIOSDriver(AppiumOptions appiumOptions)
        {
            return new(new SauceLabsEndpoint().EmusimUri(SauceUserName, SauceAccessKey), appiumOptions, TimeSpan
                .FromSeconds(240));
        }

        public void ExecuteSauceCleanupSteps(IWebDriver driver)
        {
            var isPassed = TestContext.CurrentContext.Result.Outcome.Status
                           == TestStatus.Passed;
            var script = "sauce:job-result=" + (isPassed ? "passed" : "failed");
            ((IJavaScriptExecutor) driver).ExecuteScript(script);
        }
    }
}