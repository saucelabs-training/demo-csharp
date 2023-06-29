using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace Dotnet
{
    [TestClass]
    public class SeleniumWeb
    {
        [TestMethod]
        public void GitpodWebTest()
        {
            Assert.IsTrue(true);
        }
        public IWebDriver Driver { get; set; }

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void Setup()
        {
            DriverOptions browserOptions;
            if (Environment.GetEnvironmentVariable("BROWSER_NAME") == "Firefox") {
                browserOptions = new FirefoxOptions();
            } else if (Environment.GetEnvironmentVariable("BROWSER_NAME") == "Safari") {
                browserOptions = new SafariOptions();
            } else {
                browserOptions = new ChromeOptions();
            }
            browserOptions.PlatformName = Environment.GetEnvironmentVariable("PLATFORM_NAME") ?? "Windows 11";
            browserOptions.BrowserVersion = Environment.GetEnvironmentVariable("BROWSER_VERSION") ?? "latest";

            var sauceOptions = new Dictionary<string, object>();
            sauceOptions.Add("name", TestContext.TestName);
            sauceOptions.Add("username", Environment.GetEnvironmentVariable("SAUCE_USERNAME"));
            sauceOptions.Add("accessKey", Environment.GetEnvironmentVariable("SAUCE_ACCESS_KEY"));
            sauceOptions.Add("build", Environment.GetEnvironmentVariable("BUILD"));

            browserOptions.AddAdditionalOption("sauce:options", sauceOptions);
            var region = Environment.GetEnvironmentVariable("REGION") ?? "us-west-1";
            var sauceUrl = new Uri("https://ondemand." + region + ".saucelabs.com/wd/hub");

            Driver = new RemoteWebDriver(sauceUrl, browserOptions);
        }

        [TestMethod]
        public void GitpodSauceDemoTest()
        {
            Driver.Navigate().GoToUrl("https://www.saucedemo.com");

            var usernameLocator = By.CssSelector("#user-name");
            var passwordLocator = By.CssSelector("#password");
            var submitLocator = By.CssSelector(".btn_action");

            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(15));
            wait.Until(drv => drv.FindElement(usernameLocator));

            var usernameElement = Driver.FindElement(usernameLocator);
            var passwordElement = Driver.FindElement(passwordLocator);
            var submitElement = Driver.FindElement(submitLocator);

            usernameElement.SendKeys("standard_user");
            passwordElement.SendKeys("secret_sauce");
            submitElement.Click();

            Assert.AreEqual("https://www.saucedemo.com/inventory.html", Driver.Url);
        }

        [TestCleanup]
        public void Teardown()
        {
            var isPassed = TestContext.CurrentTestOutcome == UnitTestOutcome.Passed;
            var script = "sauce:job-result=" + (isPassed ? "passed" : "failed");
            ((IJavaScriptExecutor)Driver).ExecuteScript(script);

            Driver?.Quit();
        }
    }
}