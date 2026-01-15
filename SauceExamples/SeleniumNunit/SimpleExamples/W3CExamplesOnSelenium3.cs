using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;

namespace Selenium3.Nunit.Scripts.SimpleExamples
{
    /**
     *The code samples here show how we would use Selenium 4 capabilities with
     * Selenium version 3.141.0
     *
     */
    [TestFixture]
    [Category("Selenium 4 tests")]
    public class W3CExamplesOnSelenium3
    {
        private IWebDriver _driver;
        private string _sauceUserName;
        private string _sauceAccessKey;
        private Dictionary<string, object> _sauceOptions;

        [SetUp]
        public void SetupTests()
        {
            //TODO please supply your Sauce Labs user name in an environment variable
            _sauceUserName = Environment.GetEnvironmentVariable("SAUCE_USERNAME", EnvironmentVariableTarget.User);
            //TODO please supply your own Sauce Labs access Key in an environment variable
            _sauceAccessKey = Environment.GetEnvironmentVariable("SAUCE_ACCESS_KEY", EnvironmentVariableTarget.User);
            _sauceOptions = new Dictionary<string, object>
            {
                ["username"] = _sauceUserName,
                ["accessKey"] = _sauceAccessKey
            };
        }
        [TearDown]
        public void CleanUpAfterEveryTestMethod()
        {
            var passed = TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed;
            if (_driver != null)
            {
                //all driver operations should happen here after the check
                ((IJavaScriptExecutor)_driver).ExecuteScript("sauce:job-result=" + (passed ? "passed" : "failed"));
                _driver.Quit();
            }
            //call to JIRA API
            //PUT to JIRA with status
            //create HTML reports
        }

        [Test]
        public void EdgeW3C()
        {
            var options = new EdgeOptions
            {
                BrowserVersion = "latest",
                PlatformName = "Windows 10"
                //AcceptInsecureCertificates = true //Insecure Certs are Not supported by Edge
            };

            _sauceOptions.Add("name", TestContext.CurrentContext.Test.Name);
            options.AddAdditionalCapability("sauce:options", _sauceOptions);

            _driver = new RemoteWebDriver(new Uri("https://ondemand.saucelabs.com/wd/hub"), options.ToCapabilities(),
                TimeSpan.FromSeconds(600));
            _driver.Navigate().GoToUrl("https://www.google.com");
            Assert.Pass();
        }
        [Test]
        [Ignore("Failing with authentication issue")]
        public void IEW3C()
        {
            var options = new InternetExplorerOptions
            {
                BrowserVersion = "latest",
                PlatformName = "Windows 10"
                //AcceptInsecureCertificates = true //Insecure Certs are Not supported by Edge
            };

            _sauceOptions.Add("name", TestContext.CurrentContext.Test.Name);
            options.AddAdditionalCapability("sauce:options", _sauceOptions);

            _driver = new RemoteWebDriver(new Uri("https://ondemand.saucelabs.com/wd/hub"), options.ToCapabilities(),
                TimeSpan.FromSeconds(600));
            _driver.Navigate().GoToUrl("https://www.google.com");
            Assert.Pass();
        }
        [Test]
        public void ChromeW3C()
        {
            var chromeOptions = new ChromeOptions
            {
                BrowserVersion = "latest",
                PlatformName = "Windows 10",
            };
            _sauceOptions.Add("name", TestContext.CurrentContext.Test.Name);
            chromeOptions.AddAdditionalCapability("sauce:options", _sauceOptions, true);

            _driver = new RemoteWebDriver(new Uri("https://ondemand.saucelabs.com/wd/hub"),
                chromeOptions.ToCapabilities(), TimeSpan.FromSeconds(600));
            _driver.Navigate().GoToUrl("https://www.google.com");
            Assert.Pass();
        }
        [Test]
        public void SafariW3C()
        {
            var safariOptions = new SafariOptions
            {
                BrowserVersion = "latest",
                PlatformName = "macOS 10.15"
                //AcceptInsecureCertificates = true Don't use this as Safari doesn't support Insecure certs
            };
            _sauceOptions.Add("name", TestContext.CurrentContext.Test.Name);

            safariOptions.AddAdditionalCapability("sauce:options", _sauceOptions);

            _driver = new RemoteWebDriver(new Uri("https://ondemand.saucelabs.com/wd/hub"),
                safariOptions.ToCapabilities(), TimeSpan.FromSeconds(600));
            _driver.Navigate().GoToUrl("https://www.saucedemo.com");
            Assert.Pass();
        }
    }
}
