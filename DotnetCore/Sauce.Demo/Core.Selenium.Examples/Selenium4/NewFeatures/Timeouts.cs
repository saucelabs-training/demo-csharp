using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace Core.Selenium.Examples.Selenium4.NewFeatures
{
    [TestClass]
    public class Timeouts
    {
        public IWebDriver Driver { get; set; }

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void Setup()
        {
            var browserOptions = new ChromeOptions();
            browserOptions.PlatformName = "Windows 10";
            browserOptions.BrowserVersion = "latest";

            var sauceOptions = new Dictionary<string, object>();
            sauceOptions.Add("name", TestContext.TestName);
            sauceOptions.Add("username", Environment.GetEnvironmentVariable("SAUCE_USERNAME"));
            sauceOptions.Add("accessKey", Environment.GetEnvironmentVariable("SAUCE_ACCESS_KEY"));

            browserOptions.AddAdditionalOption("sauce:options", sauceOptions);
            var sauceUrl = new Uri("https://ondemand.us-west-1.saucelabs.com/wd/hub");

            Driver = new RemoteWebDriver(sauceUrl, browserOptions);
        }

        [TestMethod]
        public void GetTimeoutsTest()
        {
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0.3);

            Assert.AreEqual(TimeSpan.FromSeconds(0.3), Driver.Manage().Timeouts().ImplicitWait);
            Assert.AreEqual(TimeSpan.FromSeconds(300), Driver.Manage().Timeouts().PageLoad);
            Assert.AreEqual(TimeSpan.FromSeconds(30), Driver.Manage().Timeouts().AsynchronousJavaScript);
        }

        [TestCleanup]
        public void Teardown()
        {
            var isPassed = TestContext.CurrentTestOutcome == UnitTestOutcome.Passed;
            var script = "sauce:job-result=" + (isPassed ? "passed" : "failed");
            ((IJavaScriptExecutor) Driver).ExecuteScript(script);

            Driver?.Quit();
        }
    }
}