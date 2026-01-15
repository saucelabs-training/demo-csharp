using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium.Support.UI;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace Selenium3.MsTest.Scripts.Onboarding
{
    [TestClass]
    public class InstantSauceTest
    {
        private IWebDriver _driver;
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void ShouldOpenOnSafari()
        {
            // Set your sauce user name and access key from environment variables
            var sauceUserName = Environment.GetEnvironmentVariable("SAUCE_USERNAME", EnvironmentVariableTarget.User);
            var sauceAccessKey = Environment.GetEnvironmentVariable("SAUCE_ACCESS_KEY", EnvironmentVariableTarget.User);

            //configure sauce labs options
            var sauceOptions = new Dictionary<string, object>
            {
                ["username"] = sauceUserName,
                ["accessKey"] = sauceAccessKey,
                ["name"] = MethodBase.GetCurrentMethod().Name
            };

            //configure the browser options
            var safariOptions = new SafariOptions
            {
                BrowserVersion = "latest",
                PlatformName = "macOS 10.15",
                //AcceptInsecureCertificates = true Don't use this as Safari doesn't support Insecure certs
            };

            //merge sauce options with browser options
            safariOptions.AddAdditionalCapability("sauce:options", sauceOptions);

            //It's not a good idea to set this value too high because if something goes wrong,
            // the test will just hang for this amount of time. 60 sec is plenty
            var connectionTimeout = TimeSpan.FromSeconds(60);
            _driver = new RemoteWebDriver(new Uri("https://ondemand.us-west-1.saucelabs.com/wd/hub"),
                safariOptions.ToCapabilities(), connectionTimeout);

            //navigate to the url of the Sauce Labs Sample app
            _driver.Navigate().GoToUrl("https://www.saucedemo.com");

            //Create an instance of a Selenium explicit wait so that we can dynamically wait for an element
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
            //wait for the user name field to be visible and store that element into a variable
            var userNameField = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[type='text']")));
            //type the user name string into the user name field
            userNameField.SendKeys("standard_user");
            //type the password into the password field
            _driver.FindElement(By.CssSelector("[type='password']")).SendKeys("secret_sauce");
            //hit Login button
            _driver.FindElement(By.CssSelector("[type='submit']")).Click();

            //Synchronize on the next page and make sure it loads
            var inventoryPageLocator =
                wait.Until(ExpectedConditions.ElementIsVisible(By.Id("inventory_container")));
            //Assert that the inventory page displayed appropriately
            Assert.IsTrue(inventoryPageLocator.Displayed);
        }

        /*
         *Below we are performing 2 critical actions. Quitting the driver and passing
         * the test result to Sauce Labs user interface.
         */
        [TestCleanup]
        public void CleanUpAfterEveryTestMethod()
        {
            //TODO always check to make sure that the driver isn't null before using it
            if(_driver is null)
                return;

            var isPassed = TestContext.CurrentTestOutcome == UnitTestOutcome.Passed;
            ((IJavaScriptExecutor)_driver).ExecuteScript("sauce:job-result=" + (isPassed ? "passed" : "failed"));
            _driver.Quit();
        }
    }
}
