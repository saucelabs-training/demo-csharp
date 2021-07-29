using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace Core.Selenium.Examples.CrossBrowser.Start
{
    [TestFixture]
    //TODO add on top of this class [TestFixtureSource(typeof(TestConfigData), nameof(TestConfigData.PopularDesktopCombinations))]
    public class CrossBrowserTests
    {
        [TearDown]
        public void CleanUpAfterEveryTestMethod()
        {
            if (Driver == null)
                return;
            ExecuteSauceCleanupSteps(Driver);
            Driver.Quit();
        }

        [SetUp]
        public void SetupDesktopTests()
        {
            SauceOptions = new Dictionary<string, object>
            {
                ["username"] = SauceUserName,
                ["accessKey"] = SauceAccessKey,
                ["name"] = TestContext.CurrentContext.Test.Name,
                ["build"] = DateTime.Now.ToString("F")
            };

            if (BrowserOptions.BrowserName == "chrome")
                ((ChromeOptions) BrowserOptions).AddAdditionalCapability("sauce:options", SauceOptions, true);
            else
                BrowserOptions.AddAdditionalCapability("sauce:options", SauceOptions);
            Driver = GetDesktopDriver(BrowserOptions.ToCapabilities());
        }


        /**
         * TODO uncomment the constructor below. It takes in values from [TestCaseSource]
         * We set those values on variables and then use those variables to create dynamic tests
         * Every data row from [TestCaseSource] is a new thread that runs in parallel
         */
        //public CrossBrowserTests(string browserVersion, string platformName, DriverOptions browserOptions)
        //{
        //    if (string.IsNullOrEmpty(browserVersion))
        //        BrowserVersion = browserVersion;
        //    if (string.IsNullOrEmpty(platformName))
        //        PlatformName = platformName;
        //    BrowserOptions = browserOptions;
        //}

        public void ExecuteSauceCleanupSteps(IWebDriver driver)
        {
            var isPassed = TestContext.CurrentContext.Result.Outcome.Status
                           == TestStatus.Passed;
            var script = "sauce:job-result=" + (isPassed ? "passed" : "failed");
            ((IJavaScriptExecutor) driver).ExecuteScript(script);
        }

        public IWebDriver Driver { get; set; }

        public string SauceUserName =>
            Environment.GetEnvironmentVariable("SAUCE_USERNAME");

        public string SauceAccessKey =>
            Environment.GetEnvironmentVariable("SAUCE_ACCESS_KEY");

        public Dictionary<string, object> SauceOptions { get; set; }

        public IWebDriver GetDesktopDriver(ICapabilities browserOptions)
        {
            return new RemoteWebDriver(new Uri("https://ondemand.saucelabs.com/wd/hub"), browserOptions);
        }

        public string BrowserVersion { get; }
        public string PlatformName { get; }
        public DriverOptions BrowserOptions { get; }



        [Test]
        [Category("cross-browser-start")]
        public void LoginWorks()
        {
            Driver.Navigate().GoToUrl("https://www.saucedemo.com");
            Driver.FindElement(By.CssSelector("#user-name")).SendKeys("standard_user");
            Driver.FindElement(By.CssSelector("#password")).SendKeys("secret_sauce");
            Driver.FindElement(By.CssSelector(".btn_action")).Click();

            Action waitForVisible = () => new Wait(Driver).UntilIsVisible(By.Id("inventory_container"));
            waitForVisible.Should().NotThrow();
        }
    }
}