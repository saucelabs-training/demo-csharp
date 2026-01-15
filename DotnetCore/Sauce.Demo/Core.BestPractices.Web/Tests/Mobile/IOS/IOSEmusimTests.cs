using Core.BestPractices.Web.MobileWebPageObjects.IOS;
using Core.Common;
using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.iOS;

namespace Core.BestPractices.Web.Tests.Mobile.IOS
{
    [TestFixture]
    [TestFixtureSource(typeof(TestConfigData), nameof(TestConfigData.PopularIOSSimulators))]
    public class IOSEmusimTests : EmusimBaseTest
    {
        [SetUp]
        public void Setup()
        {
            var appiumOptions = new AppiumOptions();
            appiumOptions.DeviceName = DeviceName;
            appiumOptions.PlatformName = "iOS";
            appiumOptions.PlatformVersion = PlatformVersion;
            appiumOptions.BrowserName = "Safari";
            appiumOptions.AddAdditionalAppiumOption("name", TestContext.CurrentContext.Test.Name);
            appiumOptions.AddAdditionalAppiumOption("build", Constants.BuildId);

            _driver = GetIOSDriver(appiumOptions);
        }

        [TearDown]
        public void EmusimTeardown()
        {
            if (_driver == null) return;

            ExecuteSauceCleanupSteps(_driver);
            _driver.Quit();
        }

        private IOSDriver _driver;

        public IOSEmusimTests(string deviceName, string platformVersion) : base(deviceName, platformVersion)
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