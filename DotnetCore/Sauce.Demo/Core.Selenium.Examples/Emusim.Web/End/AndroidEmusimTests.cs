using System;
using Core.Common;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Support.UI;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace Core.Selenium.Examples.Emusim.Web.End
{
    [TestFixture]
    [TestFixtureSource(typeof(TestConfigData), nameof(TestConfigData.AndroidSimulators))]
    [Category("emusim")]
    [Category("android-end")]
    public class AndroidEmusimTests : EmusimBaseTest
    {
        [SetUp]
        public void Setup()
        {
            var appiumOptions = new AppiumOptions();
            appiumOptions.AddAdditionalOption(MobileCapabilityType.DeviceName, DeviceName);
            appiumOptions.AddAdditionalOption(MobileCapabilityType.PlatformName, "Android");
            appiumOptions.AddAdditionalOption(MobileCapabilityType.PlatformVersion, PlatformVersion);
            appiumOptions.AddAdditionalOption(MobileCapabilityType.BrowserName, "Chrome");
            appiumOptions.AddAdditionalOption(MobileCapabilityType.AppiumVersion, "1.20.2");
            appiumOptions.AddAdditionalOption("name", TestContext.CurrentContext.Test.Name);
            appiumOptions.AddAdditionalOption("build", Constants.BuildId);

            _driver = GetAndroidDriver(appiumOptions);
        }

        [TearDown]
        public void EmusimTeardown()
        {
            if (_driver == null) return;

            ExecuteSauceCleanupSteps(_driver);
            _driver.Quit();
        }

        private AndroidDriver _driver;

        public AndroidEmusimTests(string deviceName, string platformVersion) : base(deviceName, platformVersion)
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