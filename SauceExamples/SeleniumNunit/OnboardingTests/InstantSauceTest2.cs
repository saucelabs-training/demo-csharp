using System;
using System.Threading;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Selenium3.Nunit.Scripts.OnboardingTests
{
    [TestFixture]
    [Category("InstantSauceTest"), Category("NUnit"), Category("Instant")]
    public class InstantSauceTest2
    {
        private IWebDriver _driver;
        [Test]
        public void ShouldOpenOnSafari()
        {
            /*
             * Best Practice
             * Instead of using hardcoded username and access key, you should store
             * the credentials in environment variables on your system. Not sure how to do this?
             * This document will help:
             * https://wiki.saucelabs.com/display/DOCS/Best+Practice%3A+Use+Environment+Variables+for+Authentication+Credentials
             */
            var sauceUserName =
                Environment.GetEnvironmentVariable("SAUCE_USERNAME", EnvironmentVariableTarget.User);
            var sauceAccessKey =
                Environment.GetEnvironmentVariable("SAUCE_ACCESS_KEY", EnvironmentVariableTarget.User);

            /*
             * In this section, we will configure our test to run on some specific
             * browser/os combination in Sauce Labs
             */
            var capabilities = new DesiredCapabilities();
            capabilities.SetCapability("username", sauceUserName);
            capabilities.SetCapability("accessKey", sauceAccessKey);
            capabilities.SetCapability("browserName", "Safari");
            capabilities.SetCapability("platform", "macOS 10.13");
            capabilities.SetCapability("version", "11.1");
            capabilities.SetCapability("name", TestContext.CurrentContext.Test.Name);
            _driver = new RemoteWebDriver(new Uri("http://ondemand.us-west-1.saucelabs.com:80/wd/hub"),
                capabilities, TimeSpan.FromSeconds(600));

            //TODO Please supply your publicly available web url in place of saucedemo.com
            //In the future, you will learn how to access private URLs
            _driver.Navigate().GoToUrl("https://www.saucedemo.com");

            //Hardcoded sleep just for demonstration purposes
            Thread.Sleep(10000);
            //This test will always pass for demonstration purposes
            Assert.IsTrue(true);
        }

        /*
         *Below we are performing 2 critical actions. Quitting the driver and passing
         * the test result to Sauce Labs user interface.
         */
        [TearDown]
        public void CleanUpAfterEveryTestMethod()
        {
            var passed = TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed;
            ((IJavaScriptExecutor)_driver).ExecuteScript("sauce:job-result=" + (passed ? "passed" : "failed"));
            _driver?.Quit();
        }
    }
}
