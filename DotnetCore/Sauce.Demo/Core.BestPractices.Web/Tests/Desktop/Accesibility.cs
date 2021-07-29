using System;
using System.Collections.Generic;
using Core.BestPractices.Web.DesktopWebPageObjects;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using Selenium.Axe;

namespace Core.BestPractices.Web.Tests.Desktop
{
    [TestFixture]
    public class Accesibility : WebTestsBase
    {
        [SetUp]
        public void SetupDesktopTests()
        {
            var chromeOptions = new ChromeOptions
            {
                BrowserVersion = "latest",
                PlatformName = "Windows 10",
                UseSpecCompliantProtocol = true
            };
            chromeOptions.AddAdditionalCapability("sauce:options", SauceOptions, true);
            Driver = GetDesktopDriver(chromeOptions.ToCapabilities());
        }
        [Test]
        public void AccessibilityTest()
        {
            Driver.Navigate().GoToUrl("https://www.ultimateqa.com");
            var results = Driver.Analyze();
            Assert.IsNull(results.Error);
        }
    }
}
