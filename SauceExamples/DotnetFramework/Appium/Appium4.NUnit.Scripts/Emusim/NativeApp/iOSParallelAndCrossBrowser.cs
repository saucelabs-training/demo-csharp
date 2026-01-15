using System;
using Common.SauceLabs;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Support.UI;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace Appium4.NUnit.Scripts.Emusim.NativeApp
{
    [TestFixture]
    [Parallelizable(ParallelScope.Children)]
    public class iOSParallelAndCrossBrowser
    {
        [Test]
        [Category("NativeApp")]
        [Category("iOS")]
        [TestCase("iPhone XS Simulator")]
        [TestCase("iPhone XS Simulator")]
        [TestCase("iPhone XS Simulator")]
        [TestCase("iPhone XS Simulator")]
        [TestCase("iPhone XS Simulator")]
        [TestCase("iPhone XS Simulator")]
        [TestCase("iPhone XS Simulator")]
        [TestCase("iPhone XS Simulator")]
        [TestCase("iPhone XS Simulator")]
        [TestCase("iPhone XS Simulator")]
        [TestCase("iPhone XS Simulator")]
        [TestCase("iPhone XS Simulator")]
        [TestCase("iPhone XS Simulator")]
        [TestCase("iPhone XS Simulator")]
        [TestCase("iPhone XS Simulator")]

        public void RunNativeAppOnIphone(string deviceName)
        {
            using (var scope = new TestScope(deviceName, TestContext.CurrentContext))
            {
                Assert.True(IsLoginButtonDisplayed(scope.Driver));
            }
        }

        private bool IsLoginButtonDisplayed(IOSDriver<IOSElement> driver)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            var loginButton = wait.Until(
                ExpectedConditions.ElementIsVisible(MobileBy.AccessibilityId("test-LOGIN")));
            return loginButton.Displayed;
        }

        private sealed class TestScope : IDisposable
        {
            private TestContext TestContext { get; set; }

            public IOSDriver<IOSElement> Driver { get; set; }

            public TestScope(string deviceName, TestContext testContext)
            {
                TestContext = testContext;
                var capabilities = new AppiumOptions();
                //We can run on any iPhone Device
                capabilities.AddAdditionalCapability(MobileCapabilityType.DeviceName, deviceName);
                capabilities.AddAdditionalCapability(MobileCapabilityType.PlatformName, "iOS");
                capabilities.AddAdditionalCapability(MobileCapabilityType.PlatformVersion, "13.2");

                //TODO it's a best practice to set the appium version so that you're always getting the latest
                capabilities.AddAdditionalCapability("appiumVersion", "1.16.0");
                /*
                 * !!!!!!
                 * TODO first you must upload an app to Emusim
                 * https://wiki.saucelabs.com/display/DOCS/Uploading+your+Application+to+Sauce+Storage
                 */
                // iOS apps need to be uploaded to Sauce Storage in a .zip format
                capabilities.AddAdditionalCapability("app", "sauce-storage:sample-app-ios230.zip");
                capabilities.AddAdditionalCapability("appWaitActivity", "com.swaglabsmobileapp.MainActivity");

                capabilities.AddAdditionalCapability("name", TestContext.CurrentContext.Test.Name);
                capabilities.AddAdditionalCapability("newCommandTimeout", 90);

                //60 seconds default for the connection timeout
                var sauceHubUrl = new Uri($"https://{SauceUser.Name}:{SauceUser.AccessKey}" +
                    "@ondemand.us-west-1.saucelabs.com:443/wd/hub");
                Driver = new IOSDriver<IOSElement>(sauceHubUrl, capabilities);
            }

            public void Dispose()
            {
                //clean-up code goes here
                if (Driver == null)
                    return;
                var passed = TestContext.Result.Outcome.Status == TestStatus.Passed;
                //TODO can't get this to work, it always fails
                ((IJavaScriptExecutor)Driver).ExecuteScript("sauce:job-result=" + (passed ? "passed" : "failed"));
                Driver.Quit();
            }
        }
    }
}
