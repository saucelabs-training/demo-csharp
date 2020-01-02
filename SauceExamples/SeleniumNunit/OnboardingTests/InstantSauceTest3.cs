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
    public class InstantSauceTest3
    {
        private IWebDriver _driver;
        [Test]
        public void ShouldOpenOnDifferentBrowser()
        {
            var sauceUserName =
                Environment.GetEnvironmentVariable("SAUCE_USERNAME", EnvironmentVariableTarget.User);
            var sauceAccessKey =
                Environment.GetEnvironmentVariable("SAUCE_ACCESS_KEY", EnvironmentVariableTarget.User);

            /*
             * TODO use the Platform Configurator from Sauce Labs to configure your test to run on 
             * another browser/OS combination: https://wiki.saucelabs.com/display/DOCS/Platform+Configurator#/
             * Paste that code below.
             */
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
