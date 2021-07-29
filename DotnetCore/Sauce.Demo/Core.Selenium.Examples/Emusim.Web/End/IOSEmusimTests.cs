using System;
using Core.Common;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Support.UI;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace Core.Selenium.Examples.Emusim.Web.End
{
    [TestFixture]
    [TestFixtureSource(typeof(TestConfigData), nameof(TestConfigData.IOSSimulators))]
    [Category("ios-end")]
    [Category("emusim")]
    public class IOSEmusimTests : EmusimBaseTest
    {
        [SetUp]
        public void Setup()
        {
            var appiumOptions = new AppiumOptions();
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.DeviceName, DeviceName);
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.PlatformName, "iOS");
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.PlatformVersion, PlatformVersion);
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.BrowserName, "Safari");
            appiumOptions.AddAdditionalCapability("name", TestContext.CurrentContext.Test.Name);
            appiumOptions.AddAdditionalCapability("build", Common.Constants.BuildId);

            _driver = GetIOSDriver(appiumOptions);
        }

        [TearDown]
        public void EmusimTeardown()
        {
            if (_driver == null) return;

            ExecuteSauceCleanupSteps(_driver);
            _driver.Quit();
        }

        private IOSDriver<IOSElement> _driver;

        public IOSEmusimTests(string deviceName, string platformVersion) : base(deviceName, platformVersion)
        {
        }

        [Test]
        [Retry(1)]
        public void ValidUserCanLogin()
        {
            _driver.Navigate().GoToUrl("https://www.saucedemo.com");
            // Appium doesn't accept Ids as a locator strategy. Better to use CssSelector
            _driver.FindElement(By.CssSelector("#user-name")).SendKeys("standard_user");
            _driver.FindElement(By.CssSelector("#password")).SendKeys("secret_sauce");
            _driver.FindElement(By.CssSelector(".btn_action")).Click();

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#inventory_container")));
        }
    }
}