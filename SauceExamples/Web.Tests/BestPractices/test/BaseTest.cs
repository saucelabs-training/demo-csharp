using System;
using System.Configuration;
using Common;
using Common.SauceLabs;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using RestSharp;
using RestSharp.Authenticators;

namespace Selenium3.Nunit.Framework.BestPractices.test
{
    [TestFixture]
    [Category("BestPractices")]

    public class BaseTest
    {
        private readonly string _browser;
        private readonly string _browserVersion;
        private readonly string _osPlatform;
        public SauceJavaScriptExecutor SauceReporter;
        private SauceLabsCapabilities SauceConfig { get; set; }

        [SetUp]
        public void ExecuteBeforeEveryTestMethod()
        {
            SauceConfig = new SauceLabsCapabilities
            {
                IsDebuggingEnabled = bool.Parse(ConfigurationManager.AppSettings["isExtendedDebuggingEnabled"]),
                IsHeadless = bool.Parse(ConfigurationManager.AppSettings["sauceHeadless"])
            };
            SauceLabsCapabilities.BuildName = ConfigurationManager.AppSettings["buildName"];

            Driver = new WebDriverFactory(SauceConfig).CreateSauceDriver(_browser, _browserVersion, _osPlatform);
            SauceReporter = new SauceJavaScriptExecutor(Driver);
            SauceReporter.SetTestName(TestContext.CurrentContext.Test.Name);
            SauceReporter.SetBuildName(SauceLabsCapabilities.BuildName);
        }

        [TearDown]
        public void CleanUpAfterEveryTestMethod()
        {
            if (SauceConfig.IsUsingSauceLabs) ExecuteSauceCleanupSteps();
            Driver?.Quit();
        }

        private void ExecuteSauceCleanupSteps()
        {
            var isPassed = TestContext.CurrentContext.Result.Outcome.Status
                == TestStatus.Passed;
            SauceReporter.LogTestStatus(isPassed);
            //SetTestStatusUsingApi(isPassed);
            SauceReporter.LogMessage("Test finished execution");
            SauceReporter.LogMessage(TestContext.CurrentContext.Result.Message);
        }

        private void SetTestStatusUsingApi(bool isPassed)
        {
            var userName = SauceUser.Name;
            var accessKey = SauceUser.AccessKey;

            var sessionId = ((RemoteWebDriver)Driver).SessionId;
            var client = new RestClient
            {
                Authenticator = new HttpBasicAuthenticator(userName, accessKey),
                BaseUrl = new Uri(new SauceLabsEndpoint().HeadlessRestApiUrl)
            };
            var request = new RestRequest($"/{userName}/jobs/{sessionId}",
                Method.PUT)
            { RequestFormat = DataFormat.Json };
            request.AddJsonBody(new { passed = isPassed });
            client.Execute(request);
        }



        public BaseTest(string browser, string browserVersion, string osPlatform)
        {
            _browser = browser;
            _browserVersion = browserVersion;
            _osPlatform = osPlatform;
        }

        public IWebDriver Driver { get; set; }
    }
}