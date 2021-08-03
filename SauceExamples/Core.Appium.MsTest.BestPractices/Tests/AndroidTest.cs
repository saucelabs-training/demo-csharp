using System;
using Common.SauceLabs;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;

namespace Core.Appium.Nunit.BestPractices.Tests
{
    public class AndroidTest : BaseNativeAppTest
    {
        private readonly string _androidVersion;

        private readonly string _deviceName;

        public AndroidTest(string deviceName, string deviceVersion)
        {
            _deviceName = deviceName;
            _androidVersion = deviceVersion;
        }

        public AndroidDriver<AndroidElement> Driver { get; set; }

        [SetUp]
        public void Setup()
        {
            var capabilities = new AppiumOptions();
            //We can run on any version of the platform as long as it's the correct device
            //Make sure to pick an Android or iOS device based on your app
            capabilities.AddAdditionalCapability(MobileCapabilityType.DeviceName, _deviceName);
            if (!string.IsNullOrEmpty(_androidVersion))
                capabilities.AddAdditionalCapability(MobileCapabilityType.PlatformVersion, _androidVersion);
            capabilities.AddAdditionalCapability(MobileCapabilityType.PlatformName, "Android");
            capabilities.AddAdditionalCapability("newCommandTimeout", 90);
            capabilities.AddAdditionalCapability("name", TestContext.CurrentContext.Test.Name);
            /*
             * You need to upload your own Native Mobile App to Sauce Storage!
             * https://wiki.saucelabs.com/display/DOCS/Uploading+your+Application+to+Sauce+Storage
             * You can use either storage:<app-id> or storage:filename={your file name}
             */
            capabilities.AddAdditionalCapability("app", new ApiKeys().Rdc.Apps.SampleAppAndroidGithubUrl);


            /*
             Getting Error: OpenQA.Selenium.WebDriverException : The HTTP request to the remote WebDriver server for URL 
            https://us1.appium.testobject.com/wd/hub/session timed out after 60 seconds.
                ----> System.Net.WebException : The operation has timed out
            Solution: Try changing to a more popular device
             */
            Driver = new AndroidDriver<AndroidElement>(new Uri(Url), capabilities, TimeSpan.FromSeconds(180));
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