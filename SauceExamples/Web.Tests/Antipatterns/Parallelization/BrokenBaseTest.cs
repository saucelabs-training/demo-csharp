using System.Configuration;
using Common;
using Common.SauceLabs;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;

namespace Selenium3.Nunit.Framework.Antipatterns.Parallelization
{

    [TestFixture]
    public class BrokenBaseTest
    {
        private readonly string _browser;
        private readonly string _browserVersion;
        private readonly string _osPlatform;
        public SauceJavaScriptExecutor SauceReporter;
        private SauceLabsCapabilities SauceConfig { get; set; }
        public static IWebDriver Driver { get; set; }


        public BrokenBaseTest(string browser, string browserVersion, string osPlatform)
        {
            _browser = browser;
            _browserVersion = browserVersion;
            _osPlatform = osPlatform;
        }

        [SetUp]
        public void ExecuteBeforeEveryTestMethod()
        {
            SauceConfig = new SauceLabsCapabilities
            {
                IsDebuggingEnabled = bool.Parse(ConfigurationManager.AppSettings["isExtendedDebuggingEnabled"]),
                IsHeadless = false
            };

            Driver = new WebDriverFactory(SauceConfig).CreateSauceDriver(_browser, _browserVersion, _osPlatform);
            SauceReporter = new SauceJavaScriptExecutor(Driver);
            SauceReporter.SetTestName(TestContext.CurrentContext.Test.Name);
            SauceReporter.SetBuildName("StaticParallelization");
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



    }
}