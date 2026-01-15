using System;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using RestSharp;
using RestSharp.Authenticators;

namespace Selenium3.Nunit.Scripts.SimpleExamples
{
    [TestFixture]
    [Category("SimpleTest")]
    public class RestApiForVdc
    {
        IWebDriver _driver;

        //TODO please supply your Sauce Labs user name in an environment variable
        private string sauceUserName = Environment.GetEnvironmentVariable(
            "SAUCE_USERNAME", EnvironmentVariableTarget.User);
        //TODO please supply your own Sauce Labs access Key in an environment variable
        private string sauceAccessKey = Environment.GetEnvironmentVariable(
            "SAUCE_ACCESS_KEY", EnvironmentVariableTarget.User);
        //TODO make sure that you are setting the session Id so that you can use it
        //in your API requests
        private SessionId sessionId;


        [Test]
        public void ShouldPass()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddAdditionalCapability(CapabilityType.Version, "latest", true);
            options.AddAdditionalCapability(CapabilityType.Platform, "Windows 10", true);
            options.AddAdditionalCapability("username", sauceUserName, true);
            options.AddAdditionalCapability("accessKey", sauceAccessKey, true);
            options.AddAdditionalCapability("name", TestContext.CurrentContext.Test.Name, true);

            _driver = new RemoteWebDriver(new Uri("https://ondemand.us-west-1.saucelabs.com/wd/hub"), options.ToCapabilities(),
                TimeSpan.FromSeconds(600));
            _driver.Navigate().GoToUrl("https://www.google.com");
            sessionId = ((RemoteWebDriver)_driver).SessionId;
            Assert.Pass();
        }

        [Test]
        public void ShouldFail()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddAdditionalCapability(CapabilityType.Version, "latest", true);
            options.AddAdditionalCapability(CapabilityType.Platform, "Windows 10", true);
            options.AddAdditionalCapability("username", sauceUserName, true);
            options.AddAdditionalCapability("accessKey", sauceAccessKey, true);
            options.AddAdditionalCapability("name", TestContext.CurrentContext.Test.Name, true);

            _driver = new RemoteWebDriver(new Uri("https://ondemand.us-west-1.saucelabs.com/wd/hub"), options.ToCapabilities(),
                TimeSpan.FromSeconds(600));
            _driver.Navigate().GoToUrl("https://www.google.com");
            sessionId = ((RemoteWebDriver)_driver).SessionId;
            Assert.Fail();
        }

        [TearDown]
        public void CleanUpAfterEveryTestMethod()
        {
            //End your session and stop your resources first
            _driver?.Quit();

            //How to set a test status using REST API
            var isPassed = TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed;
            var client = new RestClient
            {
                Authenticator = new HttpBasicAuthenticator(sauceUserName, sauceAccessKey),
                BaseUrl = new Uri("https://saucelabs.com/rest/v1")
            };

            //https://saucelabs.com/rest/v1/USERNAME/jobs/JOB_ID
            var request = new RestRequest($"{sauceUserName}/jobs/{sessionId}",
                Method.PUT)
            { RequestFormat = DataFormat.Json };
            request.AddJsonBody(new { passed = isPassed });
            client.Execute(request);
        }
    }
}
