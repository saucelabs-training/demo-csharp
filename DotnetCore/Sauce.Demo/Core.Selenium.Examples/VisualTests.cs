using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;

namespace Core.Selenium.Examples
{
    [TestFixture]
    public class VisualTests
    {
        private IWebDriver _driver;
        private Dictionary<string, object> _sauceOptions;

        public Dictionary<string, object> _visualOptions { get; private set; }

        private IJavaScriptExecutor JsExecutor => (IJavaScriptExecutor)_driver;

        [SetUp]
        public void SetupTests()
        {
            //TODO please supply your Sauce Labs user name in an environment variable
            var sauceUserName = Environment.GetEnvironmentVariable("SAUCE_USERNAME");
            //TODO please supply your own Sauce Labs access Key in an environment variable
            var sauceAccessKey = Environment.GetEnvironmentVariable("SAUCE_ACCESS_KEY");
            //TODO store your Screener API key in environment variables
            var screenerApiKey = Environment.GetEnvironmentVariable("SCREENER_API_KEY");

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
                UseSpecCompliantProtocol = true
            };
            _sauceOptions.Add("name", TestContext.CurrentContext.Test.Name);
            chromeOptions.AddAdditionalCapability("sauce:options", _sauceOptions, true);
            chromeOptions.AddAdditionalCapability("sauce:visual", _visualOptions, true);

            _driver = GetDriver(chromeOptions);
            _driver.Navigate().GoToUrl("https://www.saucedemo.com");

            JsExecutor.ExecuteScript("/*@visual.init*/", "Visual C# Test");
            JsExecutor.ExecuteScript("/*@visual.snapshot*/", "Login Page");

            // learn more https://docs.saucelabs.com/visual/e2e-testing/commands-options/
            //ignore an element
            var ignoredElement = new Dictionary<string, object>();
            ignoredElement.Add("ignore", "#login_button_container");
            JsExecutor.ExecuteScript("/*@visual.snapshot*/", "Ignore on Snapshot", ignoredElement);

            //cropTo
            var croppedElement = new Dictionary<string, object>();
            croppedElement.Add("cropTo", ".bot_column");
            JsExecutor.ExecuteScript("/*@visual.snapshot*/", "cropTo", croppedElement);

            var response = ((IJavaScriptExecutor) _driver).ExecuteScript("/*@visual.end*/");
            Assert.Null(response);
        }

        private IWebDriver GetDriver(DriverOptions driverOptions)
        {
            //
            return new RemoteWebDriver(new Uri("https://hub.screener.io:443/wd/hub"),
                driverOptions.ToCapabilities(), TimeSpan.FromSeconds(60));
        }
    }
}