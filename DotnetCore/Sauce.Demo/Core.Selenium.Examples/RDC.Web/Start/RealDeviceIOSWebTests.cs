using System;
using Core.Common;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Support.UI;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace Core.Selenium.Examples.RDC.Web.Start
{
    //TODO use [TestFixtureSource(typeof(TestConfigData), nameof(TestConfigData.MostPopularIOSDevices))]
    [TestFixtureSource(typeof(TestConfigData), nameof(TestConfigData.AndroidDevices))]
    public class RealDeviceIOSWebTests : MobileBaseTest
    {
        [SetUp]
        public void IOSSetup()
        {
            //TODO use GetIOSDriver()
            Driver = GetAndroidDriver(MobileOptions);
        }

        [TearDown]
        public void Teardown()
        {
            if (Driver == null) return;

            ExecuteSauceCleanupSteps(Driver);
            Driver.Quit();
        }

        //TODO replace with IOSDriver<IOSElement>
        public new AndroidDriver<AndroidElement> Driver { get; set; }

        public RealDeviceIOSWebTests(string deviceName, string platform, string browser) :
            base(deviceName, platform, browser)
        {
        }

        [Test]
        [Retry(1)]
        public void LoginWorks()
        {
            Driver.Navigate().GoToUrl("https://www.saucedemo.com");
            // Appium doesn't accept Ids as a locator strategy. Better to use CssSelector
            Driver.FindElement(By.CssSelector("#user-name")).SendKeys("standard_user");
            Driver.FindElement(By.CssSelector("#password")).SendKeys("secret_sauce");
            Driver.FindElement(By.CssSelector(".btn_action")).Click();

            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#inventory_container")));
        }
    }
}