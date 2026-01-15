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

        public IJavaScriptExecutor JsExecutor => (IJavaScriptExecutor) Driver;

        public IWebDriver GetDesktopDriver(ICapabilities browserOptions)
        {
            return new RemoteWebDriver(new Uri("https://ondemand.us-west-1.saucelabs.com/wd/hub"), browserOptions);
        }

        public AndroidDriver GetAndroidDriver(AppiumOptions appiumOptions)
        {
            return new(new SauceLabsEndpoint().EmusimUri(SauceUserName, SauceAccessKey), appiumOptions, TimeSpan
                .FromSeconds(240));
        }

        public IOSDriver GetIOSDriver(AppiumOptions appiumOptions)
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