using System;
using Common.SauceLabs;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.iOS;

namespace Core.Appium.Nunit.BestPractices.Tests
{
    public class IosTest : BaseNativeAppTest
    {
        private readonly string _deviceName;
        public IOSDriver<IOSElement> Driver;

        public IosTest(string deviceName)
        {
            _deviceName = deviceName;
        }

        [SetUp]
        public void Setup()
        {
            var appiumCaps = new AppiumOptions();
            appiumCaps.AddAdditionalCapability(MobileCapabilityType.DeviceName, _deviceName);

            appiumCaps.AddAdditionalCapability(MobileCapabilityType.PlatformName, "iOS");
            appiumCaps.AddAdditionalCapability("newCommandTimeout", 90);
            appiumCaps.AddAdditionalCapability("name", TestContext.CurrentContext.Test.Name);
            /*
             * You need to upload your own Native Mobile App to Sauce Storage!
             * https://wiki.saucelabs.com/display/DOCS/Uploading+your+Application+to+Sauce+Storage
             * You can use either storage:<app-id> or storage:filename=
             */
            appiumCaps.AddAdditionalCapability("app", new ApiKeys().Rdc.Apps.SampleAppIosGithubUrl);
            Driver = new IOSDriver<IOSElement>(new Uri(Url), appiumCaps, TimeSpan.FromSeconds(180));
        }

        [TearDown]
        public void Teardown()
        {
            if (Driver == null) return;

            var isTestPassed = TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed;
            ((IJavaScriptExecutor) Driver).ExecuteScript("sauce:job-result=" + (isTestPassed ? "passed" : "failed"));
            Driver.Quit();
        }
    }
}