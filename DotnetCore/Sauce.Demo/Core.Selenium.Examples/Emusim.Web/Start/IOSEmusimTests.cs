using System;
using Core.Common;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Support.UI;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace Core.Selenium.Examples.Emusim.Web.Start
{
    [TestFixture]
    //TODO change to [TestFixtureSource(typeof(TestConfigData), nameof(TestConfigData.IOSSimulators))]
    [TestFixtureSource(typeof(TestConfigData), nameof(TestConfigData.AndroidSimulators))]
    [Category("ios-start")]
    public class IOSEmusimTests : EmusimBaseTest
    {
        [SetUp]
        public void Setup()
        {
            //TODO use https://saucelabs.com/platform/platform-configurator#/
            var appiumOptions = new AppiumOptions();
            appiumOptions.AddAdditionalOption(MobileCapabilityType.DeviceName, DeviceName);
            // TODO change this value to run on iOS
            appiumOptions.AddAdditionalOption(MobileCapabilityType.PlatformName, "Android");
            appiumOptions.AddAdditionalOption(MobileCapabilityType.PlatformVersion, PlatformVersion);
            // TODO run on Safari
            appiumOptions.AddAdditionalOption(MobileCapabilityType.BrowserName, "Chrome");
            appiumOptions.AddAdditionalOption("name", TestContext.CurrentContext.Test.Name);
            appiumOptions.AddAdditionalOption("build", Common.Constants.BuildId);

            //TODO GetIOSDriver() instead
            _driver = GetAndroidDriver(appiumOptions);
        }

        [TearDown]
        public void EmusimTeardown()
        {
            if (_driver == null) return;

            ExecuteSauceCleanupSteps(_driver);
            _driver.Quit();
        }
        //TODO change this to use IOSDriver<IOSElement>
        private AndroidDriver _driver;

        public IOSEmusimTests(string deviceName, string platformVersion) : base(deviceName, platformVersion)
        {
        }

        [Test]
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