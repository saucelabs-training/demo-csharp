using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace Core.Selenium.Examples.Selenium4.NewFeatures
{
    [TestClass]
    public class ChromeNetwork
    
    {
        public IWebDriver Driver { get; set; }

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void Setup()
        {
            var browserOptions = new ChromeOptions();
            browserOptions.PlatformName = "Windows 10";
            browserOptions.BrowserVersion = "latest";

            var sauceOptions = new Dictionary<string, object>();
            sauceOptions.Add("name", TestContext.TestName);
            sauceOptions.Add("username", Environment.GetEnvironmentVariable("SAUCE_USERNAME"));
            sauceOptions.Add("accessKey", Environment.GetEnvironmentVariable("SAUCE_ACCESS_KEY"));

            browserOptions.AddAdditionalOption("sauce:options", sauceOptions);
            var sauceUrl = new Uri("https://ondemand.us-west-1.saucelabs.com/wd/hub");

            Driver = new RemoteWebDriver(sauceUrl, browserOptions);
        }

        [TestMethod]
        public void NetworkConditionsTest()
        {
            IHasCommandExecutor hasCommandExecutor = Driver as IHasCommandExecutor;
            var getNetworkConditionsCommandInfo = new HttpCommandInfo(HttpCommandInfo.GetCommand, "/session/{sessionId}/chromium/network_conditions");
            var setNetworkConditionsCommandInfo = new HttpCommandInfo(HttpCommandInfo.PostCommand, "/session/{sessionId}/chromium/network_conditions");
            var deleteNetworkConditionsCommandInfo = new HttpCommandInfo(HttpCommandInfo.DeleteCommand, "/session/{sessionId}/chromium/network_conditions");
            hasCommandExecutor.CommandExecutor.TryAddCommand("getNetworkConditions", getNetworkConditionsCommandInfo);
            hasCommandExecutor.CommandExecutor.TryAddCommand("setNetworkConditions", setNetworkConditionsCommandInfo);
            hasCommandExecutor.CommandExecutor.TryAddCommand("deleteNetworkConditions", deleteNetworkConditionsCommandInfo);

            SessionId sessionId = ((RemoteWebDriver)Driver).SessionId;
            
            var getNetworkConditionsCommand = new Command(sessionId, "getNetworkConditions", null);
            var deleteNetworkConditionsCommand = new Command(sessionId, "deleteNetworkConditions", null);

            Dictionary<String, Object> conditions = new Dictionary<string, object>();
            conditions.Add("latency", 0);
            conditions.Add("throughput", 10000);
            conditions.Add("offline", true);
            Dictionary<String, Object> conditionsDict = new Dictionary<string, object>();
            conditionsDict.Add("network_conditions", conditions);

            var setNetworkConditionsCommand = new Command(sessionId, "setNetworkConditions", conditionsDict);
            hasCommandExecutor.CommandExecutor.Execute(setNetworkConditionsCommand);

            Response response = hasCommandExecutor.CommandExecutor.Execute(getNetworkConditionsCommand);
            Dictionary<String, Object> actual = (Dictionary<string, object>)response.Value;
            Assert.AreEqual( Convert.ToInt64(10000), actual.GetValueOrDefault("download_throughput"));
            Assert.AreEqual(Convert.ToInt64(10000), actual.GetValueOrDefault("upload_throughput"));

            try
            {
                Driver.Navigate().GoToUrl("https://www.saucedemo.com");
                Assert.Fail("Navigation should be offline, so the previous command should have thrown exception");
            }
            catch (WebDriverException)
            {
                // This is expected when offline
            }

            hasCommandExecutor.CommandExecutor.Execute(deleteNetworkConditionsCommand);

            try
            {
                Driver.Navigate().GoToUrl("https://www.saucedemo.com");
            }
            catch (WebDriverException)
            {
                Assert.Fail("Network should be back online, so no exception should have thrown exception");
            }
        }

        [TestCleanup]
        public void Teardown()
        {
            var isPassed = TestContext.CurrentTestOutcome == UnitTestOutcome.Passed;
            var script = "sauce:job-result=" + (isPassed ? "passed" : "failed");
            ((IJavaScriptExecutor) Driver).ExecuteScript(script);

            Driver?.Quit();
        }
    }
}