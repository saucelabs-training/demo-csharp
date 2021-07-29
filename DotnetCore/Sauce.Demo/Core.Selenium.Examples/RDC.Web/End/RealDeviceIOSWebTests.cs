using System;
using Core.Common;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Support.UI;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace Core.Selenium.Examples.RDC.Web.End
{
    [TestFixtureSource(typeof(TestConfigData), nameof(TestConfigData.MostPopularIOSDevices))]
    [Category("rdc")]
    [Category("ios-end")]
    public class RealDeviceIOSWebTests : MobileBaseTest
    {
        [SetUp]
        public void IOSSetup()
        {
            Driver = GetIOSDriver(MobileOptions);
        }

        [TearDown]
        public void Teardown()
        {
            if (Driver == null) return;

            ExecuteSauceCleanupSteps(Driver);
            Driver.Quit();
        }

        public new IOSDriver<IOSElement> Driver { get; set; }

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