using Core.BestPractices.Web.MobileWebPageObjects.Android;
using Core.Common;
using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;

namespace Core.BestPractices.Web.Tests.Mobile.Android
{
    [TestFixture]
    [TestFixtureSource(typeof(TestConfigData), nameof(TestConfigData.PopularAndroidSimulators))]
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
        public void LoginPageOpens()
        {
            var loginPage = new LoginPage(_driver);
            loginPage.Visit();
            loginPage.IsVisible().Should().NotThrow();
        }
    }
}