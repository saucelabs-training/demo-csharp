using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace Selenium3.MsTest.Scripts.ParallelTests
{
    [TestClass]
    public class ParallelSeleniumMethods
    {
        /*
         * How to execute parallel tests at the method level using MsTest
         *
         * Make sure that your AssemplyInfo.cs for the project has this property:
         * [assembly: Parallelize(Workers = 100, Scope = ExecutionScope.MethodLevel)]
         *
         * There are recommendations on the web to configure the .runsettings file,
         * but you do not need it to run in parallel.
         *
         * In this example, we can run many Selenium test methods in parallel without any issue
         */
        private IWebDriver _driver;
        private string _sauceUserName;
        private string _sauceAccessKey;
        private Dictionary<string, object> _sauceOptions;
        private Uri SeleniumHub => new Uri("https://ondemand.us-west-1.saucelabs.com/wd/hub");

        public TestContext TestContext { get; set; }
        [TestInitialize]
        public void SetupTests()
        {
            _sauceUserName = Environment.GetEnvironmentVariable("SAUCE_USERNAME", EnvironmentVariableTarget.Machine);
            _sauceAccessKey = Environment.GetEnvironmentVariable("SAUCE_ACCESS_KEY", EnvironmentVariableTarget.Machine);
            _sauceOptions = new Dictionary<string, object>
            {
                ["username"] = _sauceUserName,
                ["accessKey"] = _sauceAccessKey,
                ["name"] = TestContext.TestName
            };
            var chromeOptions = new ChromeOptions
            {
                BrowserVersion = "latest",
                PlatformName = "Windows 10"
            };
            chromeOptions.AddAdditionalCapability("sauce:options", _sauceOptions);

            _driver = new RemoteWebDriver(SeleniumHub,
                chromeOptions.ToCapabilities(), TimeSpan.FromSeconds(60));
        }
        [TestCleanup]
        public void CleanUpAfterEveryTestMethod()
        {
            if (_driver == null) return;

            var isPassed = TestContext.CurrentTestOutcome == UnitTestOutcome.Passed;
            ((IJavaScriptExecutor)_driver).ExecuteScript("sauce:job-result=" + (isPassed ? "passed" : "failed"));
            _driver.Quit();
        }

        public void SimpleTest()
        {
            _driver.Navigate().GoToUrl("https://www.saucedemo.com");
            Assert.AreEqual("Swag Labs", _driver.Title);
        }

        [TestMethod]
        public void SeleniumTest1()
        {
            SimpleTest();
        }
        [TestMethod]
        public void SeleniumTest2()
        {
            SimpleTest();
        }
        [TestMethod]
        public void SeleniumTest3()
        {
            SimpleTest();
        }
        [TestMethod]
        public void SeleniumTest4()
        {
            SimpleTest();
        }
        [TestMethod]
        public void SeleniumTest5()
        {
            SimpleTest();
        }
        [TestMethod]
        public void SeleniumTest6()
        {
            SimpleTest();
        }
        [TestMethod]
        public void SeleniumTest7()
        {
            SimpleTest();
        }
    }
}
