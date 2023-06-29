using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.iOS;

namespace Dotnet
{
    [TestClass]
    public class Appium
    {
        [TestMethod]
        public void GitpodAppiumTest()
        {
            Assert.IsTrue(true);
        }
        private static IWebDriver driver;

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void Setup()
        {
                // Set Sauce Labs desired capabilities
            var sauceOptions = new AppiumOptions();
            sauceOptions.AddAdditionalCapability(MobileCapabilityType.PlatformName, "iOS");
            sauceOptions.AddAdditionalCapability(MobileCapabilityType.PlatformVersion, "13.2");
            sauceOptions.AddAdditionalCapability(MobileCapabilityType.DeviceName, "iPhone Simulator");
            sauceOptions.AddAdditionalCapability("app", "sauce-storage:your-app-file.ipa");
            sauceOptions.AddAdditionalCapability("username", "your-sauce-username");
            sauceOptions.AddAdditionalCapability("accessKey", "your-sauce-access-key");

            // Set Appium server URL
            var serverUri = new Uri("https://ondemand.saucelabs.com:443/wd/hub");

            // Create Appium iOS driver
            driver = new IOSDriver<IWebElement>(serverUri, sauceOptions);
        }

        [TestCleanup]
        public void Teardown()
        {
            var isPassed = TestContext.CurrentTestOutcome == UnitTestOutcome.Passed;
            var script = "sauce:job-result=" + (isPassed ? "passed" : "failed");
            ((IJavaScriptExecutor)driver).ExecuteScript(script);

            driver?.Quit();
        }
    }
}