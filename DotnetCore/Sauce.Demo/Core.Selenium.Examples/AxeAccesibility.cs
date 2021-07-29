using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using Selenium.Axe;

namespace Core.Selenium.Examples
{
    [TestClass]
    public class AxeAccesibility
    {
        IWebDriver _webDriver;
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void AccessibilityTest()
        {
            var browserOptions = new ChromeOptions
            {
                UseSpecCompliantProtocol = true,
                PlatformName = "Windows 10",
                BrowserVersion = "latest"
            };
            var sauceOptions = new Dictionary<string, object>
            {
                { "name", TestContext.TestName },
                { "username", Environment.GetEnvironmentVariable("SAUCE_USERNAME") },
                { "accessKey", Environment.GetEnvironmentVariable("SAUCE_ACCESS_KEY") }
            };
            browserOptions.AddAdditionalCapability("sauce:options", sauceOptions, true);
            _webDriver = new RemoteWebDriver(new Uri("https://ondemand.us-west-1.saucelabs.com/wd/hub"), browserOptions);
            _webDriver.Navigate().GoToUrl("https://www.saucedemo.com");
            var results = _webDriver.Analyze();
            Assert.IsNull(results.Error);
        }
        [TestCleanup]
        public void Teardown()
        {
            _webDriver?.Quit();
        }
    }
}
