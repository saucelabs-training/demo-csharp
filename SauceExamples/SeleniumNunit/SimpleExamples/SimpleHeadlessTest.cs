using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace Selenium3.Nunit.Scripts.SimpleExamples
{
    [TestFixture]
    [Category("SimpleTest")]
    public class SimpleHeadlessTest
    {
        IWebDriver _driver;

        [Test]
        public void ChromeHeadlessW3C()
        {
            var sauceOptions = new Dictionary<string, object>
            {
                ["username"] = Environment.GetEnvironmentVariable("SAUCE_USERNAME", EnvironmentVariableTarget.User),
                ["accessKey"] = Environment.GetEnvironmentVariable("SAUCE_ACCESS_KEY", EnvironmentVariableTarget.User)
            };

            var chromeOptions = new ChromeOptions
            {
                BrowserVersion = "latest",
                PlatformName = "Linux",
            };
            sauceOptions.Add("name", TestContext.CurrentContext.Test.Name);
            chromeOptions.AddAdditionalCapability("sauce:options", sauceOptions, true);

            _driver = new RemoteWebDriver(new Uri("https://ondemand.us-east-1.saucelabs.com/wd/hub"),
                chromeOptions.ToCapabilities(), TimeSpan.FromSeconds(600));
            _driver.Navigate().GoToUrl("https://www.saucedemo.com");
            Assert.Pass();
        }

        [TearDown]
        public void CleanUpAfterEveryTestMethod()
        {
            var passed = TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed;
            ((IJavaScriptExecutor)_driver).ExecuteScript("sauce:job-result=" + (passed ? "passed" : "failed"));
            _driver?.Quit();
        }
    }
}
