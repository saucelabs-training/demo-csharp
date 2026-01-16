using System.Collections.Generic;
using Core.BestPractices.Web.DesktopWebPageObjects;
using Core.Common;
using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;

namespace Core.BestPractices.Web.Tests.Mobile.Android
{
    [TestFixtureSource(typeof(TestConfigData), nameof(TestConfigData.MostPopularAndroidDevices))]
    [Parallelizable]
    public class RealDeviceAndroidWebTests : MobileBaseTest
    {
        [SetUp]
        public void AndroidSetup()
        {
            var appiumOptions = new AppiumOptions();
            appiumOptions.AutomationName = "UiAutomator2";
            appiumOptions.DeviceName = DeviceName;
            appiumOptions.PlatformName = "Android";
            appiumOptions.BrowserName = "Chrome";
            appiumOptions.PlatformVersion = Platform;
            
            var sauceOptions = new Dictionary<string, object>(); 
            sauceOptions.Add("appiumVersion", "latest");
            sauceOptions.Add("build", Constants.BuildId);
            sauceOptions.Add("name", TestContext.CurrentContext.Test.Name);            
            appiumOptions.AddAdditionalAppiumOption("sauce:options", sauceOptions);            
            Driver = GetAndroidDriver(appiumOptions);
        }

        [TearDown]
        public void Teardown()
        {
            if (Driver == null) return;

            ExecuteSauceCleanupSteps(Driver);
            Driver.Quit();
        }

        public new AndroidDriver Driver { get; set; }

        public RealDeviceAndroidWebTests(string deviceName, string platform) :
            base(deviceName, platform)
        {
        }

        [Test]
        [Retry(1)]
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