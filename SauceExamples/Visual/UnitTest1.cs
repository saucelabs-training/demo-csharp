using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;

namespace WebDriver_CSharp_Example
{
    [TestFixture]
    public class VisualE2ETest
    {
        RemoteWebDriver driver;
        public string sauceUserName = Environment.GetEnvironmentVariable("SAUCE_USERNAME");
        public string sauceAccessKey = Environment.GetEnvironmentVariable("SAUCE_ACCESS_KEY");
        public string screenerApiKey = Environment.GetEnvironmentVariable("SCREENER_API_KEY");
        public string seleniumProtocol = Environment.GetEnvironmentVariable("SELENIUM_PROTOCOL");
        public string seleniumHost = Environment.GetEnvironmentVariable("SELENIUM_HOST");
        public string seleniumPort = Environment.GetEnvironmentVariable("SELENIUM_PORT");

        [Test(Description = "Visual E2E Test against screener.io")]
        public void testVisualE2E()
        {
            driver.Navigate().GoToUrl("https://screener.io");
            driver.ExecuteScript("/*@visual.init*/", "My Visual C# Test");
            driver.ExecuteScript("/*@visual.snapshot*/", "Home");
            var response = driver.ExecuteScript("/*@visual.end*/") as Dictionary<string, object>;
            Assert.IsTrue((Boolean)response["passed"]);
            Assert.IsNotEmpty((String)response["message"]);
        }

        [TearDown]
        public void TearDownTest()
        {
            driver.Quit();
        }


        [SetUp]
        public void SetupTest()
        {

            if (String.IsNullOrEmpty(sauceUserName))
            {
                throw new Exception("SAUCE_USERNAME environment variable needs to be defined");
            }

            if (String.IsNullOrEmpty(sauceAccessKey))
            {
                throw new Exception("SAUCE_ACCESS_KEY environment variable needs to be defined");
            }

            if (String.IsNullOrEmpty(screenerApiKey))
            {
                throw new Exception("SCREENER_API_KEY environment variable needs to be defined");
            }

            if (String.IsNullOrEmpty(seleniumProtocol))
            {
                throw new Exception("SELENIUM_PROTOCOL environment variable needs to be defined");
            }

            if (String.IsNullOrEmpty(seleniumHost))
            {
                throw new Exception("SELENIUM_HOST environment variable needs to be defined");
            }

            if (String.IsNullOrEmpty(seleniumPort))
            {
                throw new Exception("SELENIUM_PORT environment variable needs to be defined");
            }

            var chromeOptions = new ChromeOptions()
            {
                BrowserVersion = "latest",
                PlatformName = "Windows 10",
                UseSpecCompliantProtocol = true
            };

            var sauceOptions = new Dictionary<string, object>
            {
                { "username", sauceUserName },
                { "accesskey", sauceAccessKey },
                { "name", TestContext.CurrentContext.Test.Name }
            };

            var visualOptions = new Dictionary<string, object>
            {
                { "apiKey",screenerApiKey },
                { "projectName", "visual-e2e-test" },
                { "viewportSize", "1280x1024" }
            };

            chromeOptions.AddAdditionalCapability("sauce:options", sauceOptions, true);
            chromeOptions.AddAdditionalCapability("sauce:visual", visualOptions, true);

            driver = new RemoteWebDriver(new Uri(String.Format("{0}://{1}:{2}/wd/hub", seleniumProtocol, seleniumHost, seleniumPort)), chromeOptions.ToCapabilities(), TimeSpan.FromSeconds(600));
        }
    }
}
