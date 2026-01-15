using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;

namespace Selenium3.Nunit.Scripts.Visual
{
    /**
     *The code samples here show how we would use Selenium 4 capabilities with
     * Selenium version 3.141.0
     *
     */
    [TestFixture]
    [Category("Selenium 4 tests")]
    public class VisualTestSimple
    {
        private IWebDriver _driver;
        private Dictionary<string, object> _sauceOptions;

        public Dictionary<string, object> _visualOptions { get; private set; }

        private IJavaScriptExecutor JsExecutor => (IJavaScriptExecutor)_driver;

        [SetUp]
        public void SetupTests()
        {
            //TODO please supply your Sauce Labs user name in an environment variable
            var sauceUserName = Environment.GetEnvironmentVariable("SAUCE_USERNAME", EnvironmentVariableTarget.User);
            //TODO please supply your own Sauce Labs access Key in an environment variable
            var sauceAccessKey = Environment.GetEnvironmentVariable("SAUCE_ACCESS_KEY", EnvironmentVariableTarget.User);
            //TODO store your Screener API key in environment variables
            var screenerApiKey = Environment.GetEnvironmentVariable("SCREENER_API_KEY", EnvironmentVariableTarget.User);

            _sauceOptions = new Dictionary<string, object>
            {
                ["username"] = sauceUserName,
                ["accessKey"] = sauceAccessKey
            };

            _visualOptions = new Dictionary<string, object>
            {
                { "apiKey",screenerApiKey },
                { "projectName", "visual-e2e-test" },
                { "viewportSize", "1280x1024" }
            };
        }
        [TearDown]
        public void CleanUpAfterEveryTestMethod()
        {
            if (_driver != null)
            {
                //all driver operations should happen here after the check
                var isPassed = TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed;
                JsExecutor.ExecuteScript("sauce:job-result=" + (isPassed ? "passed" : "failed"));
                _driver.Quit();
            }
        }

        [Test]
        public void VisualTestOnChrome()
        {
            var chromeOptions = new ChromeOptions
            {
                BrowserVersion = "latest",
                PlatformName = "Windows 10",
            };
            _sauceOptions.Add("name", TestContext.CurrentContext.Test.Name);
            chromeOptions.AddAdditionalCapability("sauce:options", _sauceOptions, true);
            chromeOptions.AddAdditionalCapability("sauce:visual", _visualOptions, true);

            _driver = GetDriver(chromeOptions);
            _driver.Navigate().GoToUrl("https://www.saucedemo.com");

            JsExecutor.ExecuteScript("/*@visual.init*/", "Visual C# Test");
            JsExecutor.ExecuteScript("/*@visual.snapshot*/", "Login Page");
        }

        [Test]
        public void SafariW3C()
        {
            var safariOptions = new SafariOptions
            {
                BrowserVersion = "latest",
                PlatformName = "macOS 10.15"
                // Don't use AcceptInsecureCertificates, as Safari doesn't support Insecure certs
                //AcceptInsecureCertificates = true 
            };
            _sauceOptions.Add("name", TestContext.CurrentContext.Test.Name);

            safariOptions.AddAdditionalCapability("sauce:options", _sauceOptions);
            safariOptions.AddAdditionalCapability("sauce:visual", _visualOptions);

            _driver = GetDriver(safariOptions);

            _driver.Navigate().GoToUrl("https://www.saucedemo.com");

            JsExecutor.ExecuteScript("/*@visual.init*/", "Visual C# Test");
            JsExecutor.ExecuteScript("/*@visual.snapshot*/", "Login Page");
        }

        private IWebDriver GetDriver(DriverOptions driverOptions)
        {
            //
            return new RemoteWebDriver(new Uri("https://hub.screener.io:443/wd/hub"),
                driverOptions.ToCapabilities(), TimeSpan.FromSeconds(60));
        }
    }
}
