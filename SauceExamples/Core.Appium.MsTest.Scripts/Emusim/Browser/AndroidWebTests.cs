using System;
using Common.SauceLabs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Remote;

namespace Core.Appium.Examples.Emusim.Browser
{
    [TestClass]
    public class AndroidWebTests
    {
        private RemoteWebDriver _driver;
        private string _sauceUserName;
        private string _sauceAccessKey;
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void Setup()
        {
            _sauceUserName = Environment.GetEnvironmentVariable("SAUCE_USERNAME", EnvironmentVariableTarget.User);
            _sauceAccessKey = Environment.GetEnvironmentVariable("SAUCE_ACCESS_KEY", EnvironmentVariableTarget.User);

            var appiumOptions = new AppiumOptions();
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.DeviceName, "Samsung Galaxy S9 WQHD GoogleAPI Emulator");
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.PlatformName, "Android");
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.PlatformVersion, "9.0");
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.BrowserName, "Chrome");
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.AppiumVersion, "1.9.1");
            appiumOptions.AddAdditionalCapability("name", TestContext.TestName);

            _driver = new RemoteWebDriver(new SauceLabsEndpoint().SauceUri(_sauceUserName, _sauceAccessKey),
                appiumOptions.ToCapabilities(), TimeSpan.FromSeconds(120));
        }
        [TestCleanup]
        public void Teardown()
        {
            if (_driver == null) return;

            var isPassed = TestContext.CurrentTestOutcome == UnitTestOutcome.Passed;
            ((IJavaScriptExecutor)_driver).ExecuteScript("sauce:job-result=" + (isPassed ? "passed" : "failed"));
            _driver.Quit();
        }

        [TestMethod]
        public void ShouldOpenBrowser()
        {
            _driver.Navigate().GoToUrl("https://www.saucedemo.com");
            Assert.AreNotEqual(0, _driver.Manage().Window.Size);
        }
    }
}
