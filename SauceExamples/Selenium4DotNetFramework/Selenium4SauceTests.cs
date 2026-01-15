using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;

namespace Selenium4.MsTest.Scripts
{
    [TestClass]
    [TestCategory("WebDriver 4 tests on Sauce")]
    public class Selenium4SauceTests
    {
        IWebDriver _driver;
        private string sauceUserName;
        private string sauceAccessKey;
        private Dictionary<string, object> sauceOptions;
        public TestContext TestContext { get; set; }

        [TestCleanup]
        public void CleanUpAfterEveryTestMethod()
        {
            var passed = TestContext.CurrentTestOutcome == UnitTestOutcome.Passed;
            if (_driver == null) { return;}
            
            ((IJavaScriptExecutor)_driver).ExecuteScript("sauce:job-result=" + (passed ? "passed" : "failed"));
            _driver.Quit();
        }

        [TestMethod]
        public void EdgeW3C()
        {
            var options = new EdgeOptions
            {
                BrowserVersion = "latest",
                PlatformName = "Windows 10"
                //AcceptInsecureCertificates = true //Insecure Certs are Not supported by Edge
            };

            sauceOptions.Add("name", MethodBase.GetCurrentMethod().Name);
            options.AddAdditionalOption("sauce:options", sauceOptions);

            _driver = new RemoteWebDriver(new Uri("https://ondemand.us-west-1.saucelabs.com/wd/hub"), options.ToCapabilities(),
                TimeSpan.FromSeconds(30));
            GoToThenAssert();
        }
        [TestMethod]
        public void IEW3C()
        {
            var options = new InternetExplorerOptions
            {
                BrowserVersion = "latest",
                PlatformName = "Windows 10"
                //AcceptInsecureCertificates = true //Insecure Certs are Not supported by Edge
            };

            sauceOptions.Add("name", MethodBase.GetCurrentMethod().Name);
            options.AddAdditionalOption("sauce:options", sauceOptions);

            _driver = new RemoteWebDriver(new Uri("https://ondemand.us-west-1.saucelabs.com/wd/hub"), options.ToCapabilities(),
                TimeSpan.FromSeconds(30));
            GoToThenAssert();
        }

        private void GoToThenAssert()
        {
            _driver.Navigate().GoToUrl("https://www.saucedemo.com");
            Assert.IsTrue(_driver.Url.Contains("saucedemo.com"));
        }

        [TestMethod]
        public void ChromeW3C()
        {
            var chromeOptions = new ChromeOptions
            {
                BrowserVersion = "latest",
                PlatformName = "Windows 10"
            };
            sauceOptions.Add("name", MethodBase.GetCurrentMethod().Name);
            chromeOptions.AddAdditionalOption("sauce:options", sauceOptions);

            _driver = new RemoteWebDriver(new Uri("https://ondemand.us-west-1.saucelabs.com/wd/hub"),
                chromeOptions.ToCapabilities(), TimeSpan.FromSeconds(30));
            GoToThenAssert();
        }
        [TestMethod]
        public void SafariW3C()
        {
            SafariOptions safariOptions = new SafariOptions
            {
                BrowserVersion = "12.0",
                PlatformName = "macOS 10.13"
                //AcceptInsecureCertificates = true Don't use this as Safari doesn't support Insecure certs
            };
            sauceOptions.Add("name", MethodBase.GetCurrentMethod().Name);
            safariOptions.AddAdditionalOption("sauce:options", sauceOptions);

            _driver = new RemoteWebDriver(new Uri("https://ondemand.us-west-1.saucelabs.com/wd/hub"),
                safariOptions.ToCapabilities(), TimeSpan.FromSeconds(30));
            GoToThenAssert();
        }
        [TestMethod]
        public void FirefoxW3C()
        {
            var browserOptions = new FirefoxOptions
            {
                BrowserVersion = "latest",
                PlatformName = "Windows 10"
            };
            sauceOptions.Add("name", MethodBase.GetCurrentMethod().Name);
            browserOptions.AddAdditionalOption("sauce:options", sauceOptions);

            _driver = new RemoteWebDriver(new Uri("https://ondemand.us-west-1.saucelabs.com/wd/hub"),
                browserOptions.ToCapabilities(), TimeSpan.FromSeconds(30));
            _driver.Navigate().GoToUrl("https://www.saucedemo.com");
            GoToThenAssert();
        }
        [TestMethod]
        public void EdgeChromiumW3C()
        {
            var browserOptions = new EdgeOptions()
            {
                BrowserVersion = "latest",
                PlatformName = "macOS 10.14"
            };
            sauceOptions.Add("name", MethodBase.GetCurrentMethod().Name);
            browserOptions.AddAdditionalOption("sauce:options", sauceOptions);

            _driver = new RemoteWebDriver(new Uri("https://ondemand.us-west-1.saucelabs.com/wd/hub"),
                browserOptions.ToCapabilities(), TimeSpan.FromSeconds(30));
            _driver.Navigate().GoToUrl("https://www.saucedemo.com");
            GoToThenAssert();
        }
        [TestInitialize]
        public void SetupTests()
        {
            //TODO please supply your Sauce Labs user name in an environment variable
            sauceUserName = Environment.GetEnvironmentVariable("SAUCE_USERNAME", EnvironmentVariableTarget.User);
            //TODO please supply your own Sauce Labs access Key in an environment variable
            sauceAccessKey = Environment.GetEnvironmentVariable("SAUCE_ACCESS_KEY", EnvironmentVariableTarget.User);
            sauceOptions = new Dictionary<string, object>
            {
                ["username"] = sauceUserName,
                ["accessKey"] = sauceAccessKey
            };
        }

    }
}
