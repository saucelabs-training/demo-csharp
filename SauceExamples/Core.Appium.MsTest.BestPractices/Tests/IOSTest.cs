using System;
using System.Collections.Generic;
using Common.SauceLabs;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.iOS;

namespace Core.Appium.Nunit.BestPractices.Tests
{
    public class IosTest : BaseNativeAppTest
    {
        private readonly string _deviceName;

        protected IOSDriver Driver { get; set; }

        public IosTest(string deviceName)
        {
            _deviceName = deviceName;
        }

        [SetUp]
        public void Setup()
        {
            var appiumCaps = new AppiumOptions();
            appiumCaps.DeviceName = _deviceName;
            appiumCaps.PlatformName = "iOS";
            appiumCaps.AutomationName = "XCUITest";
            appiumCaps.App = new ApiKeys().Rdc.Apps.SampleAppIosGithubUrl;
            
            // Put all Sauce Labs specific capabilities in sauce:options
            var sauceOptions = new Dictionary<string, object>
            {
                ["name"] = TestContext.CurrentContext.Test.Name,
                ["newCommandTimeout"] = 90,
                ["appiumVersion"] = "latest"
            };
            
            appiumCaps.AddAdditionalAppiumOption("sauce:options", sauceOptions);
            Driver = new IOSDriver(new Uri(Url), appiumCaps, TimeSpan.FromSeconds(180));
        }

        [TearDown]
        public void Teardown()
        {
            if (Driver == null) return;

            var isTestPassed = TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed;
            ((IJavaScriptExecutor) Driver).ExecuteScript("sauce:job-result=" + (isTestPassed ? "passed" : "failed"));
            Driver.Quit();
        }
    }
}