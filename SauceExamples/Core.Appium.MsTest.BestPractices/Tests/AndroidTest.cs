using System;
using System.Collections.Generic;
using Common.SauceLabs;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;

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

        public AndroidDriver Driver { get; set; }

        [SetUp]
        public void Setup()
        {
            var capabilities = new AppiumOptions();
            //We can run on any version of the platform as long as it's the correct device
            //Make sure to pick an Android or iOS device based on your app
            // Use properties for standard W3C capabilities
            // Use properties instead of AddAdditionalAppiumOption for standard capabilities
            capabilities.DeviceName = _deviceName;
            if (!string.IsNullOrEmpty(_androidVersion))
                capabilities.PlatformVersion = _androidVersion;
            capabilities.PlatformName = "Android";
            capabilities.AutomationName = "UiAutomator2";
            capabilities.App = new ApiKeys().Rdc.Apps.SampleAppAndroidGithubUrl;
            // Put all Sauce Labs specific capabilities in sauce:options
            var sauceOptions = new Dictionary<string, object>
            {
                ["name"] = TestContext.CurrentContext.Test.Name,
                ["newCommandTimeout"] = 90,
                /*
                 * You need to upload your own Native Mobile App to Sauce Storage!
                 * https://wiki.saucelabs.com/display/DOCS/Uploading+your+Application+to+Sauce+Storage
                 * You can use either storage:<app-id> or storage:filename={your file name}
                 */
            };
            
            capabilities.AddAdditionalAppiumOption("sauce:options", sauceOptions);


            /*
             Getting Error: OpenQA.Selenium.WebDriverException : The HTTP request to the remote WebDriver server for URL 
            https://us1.appium.testobject.com/wd/hub/session timed out after 60 seconds.
                ----> System.Net.WebException : The operation has timed out
            Solution: Try changing to a more popular device
             */
            Driver = new AndroidDriver(new Uri(Url), capabilities, TimeSpan.FromSeconds(180));
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