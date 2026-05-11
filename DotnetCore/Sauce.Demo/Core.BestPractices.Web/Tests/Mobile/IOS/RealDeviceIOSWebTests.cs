using System.Collections.Generic;
using Core.BestPractices.Web.DesktopWebPageObjects;
using Core.Common;
using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.iOS;

namespace Core.BestPractices.Web.Tests.Mobile.IOS
{
    [TestFixtureSource(typeof(TestConfigData), nameof(TestConfigData.MostPopularIOSDevices))]
    [Parallelizable]
    public class RealDeviceIOSWebTests : MobileBaseTest
    {
        [SetUp]
        public void IOSSetup()
        {
            var appiumOptions = new AppiumOptions();
            appiumOptions.AutomationName = "XCUITest";
            appiumOptions.DeviceName = DeviceName;
            appiumOptions.PlatformName = "iOS";
            appiumOptions.BrowserName = "Safari";
            appiumOptions.PlatformVersion = Platform;
            appiumOptions.AddAdditionalAppiumOption("appium:webviewConnectTimeout", 15000);
            
            SauceOptions = new Dictionary<string, object>
            {
                ["name"] = TestContext.CurrentContext.Test.Name,
                ["build"] = Constants.BuildId,
                ["appiumVersion"] = "latest"
            };            
            appiumOptions.AddAdditionalAppiumOption("sauce:options", SauceOptions);
            
            Driver = GetIOSDriver(appiumOptions);
        }

        [TearDown]
        public void Teardown()
        {
            if (Driver == null) return;

            ExecuteSauceCleanupSteps(Driver);
            Driver.Quit();
        }

        public new IOSDriver Driver { get; set; }

        public RealDeviceIOSWebTests(string deviceName, string platform) :
            base(deviceName, platform)
        {
        }

        [Test]
        public void ShouldOpenHomePage()
        {
            var loginPage = new LoginPage(Driver);
            loginPage.Visit();
            loginPage.IsVisible().Should().NotThrow();
        }
        [Test]
        [Retry(1)]
        public void LoginWorks()
        {
            var loginPage = new LoginPage(Driver);
            loginPage.Visit();
            loginPage.Login("standard_user");
            new ProductsPage(Driver).IsVisible().Should().NotThrow();
        }
    }
}