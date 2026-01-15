using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Selenium3.MsTest.Scripts
{
    [TestClass]
    [TestCategory("SimpleTest")]
    public class ExtendedDebugging
    {
        IWebDriver _driver;
        public TestContext TestContext { get; set; }
        [TestMethod]
        [Obsolete]
        public void ExtendedDebugTest()
        {
            var sauceUserName = Environment.GetEnvironmentVariable(
                "SAUCE_USERNAME", EnvironmentVariableTarget.User);
            var sauceAccessKey = Environment.GetEnvironmentVariable(
                "SAUCE_ACCESS_KEY", EnvironmentVariableTarget.User);


            DesiredCapabilities caps = new DesiredCapabilities();
            caps.SetCapability("browserName", "Chrome");
            caps.SetCapability("platform", "Windows 10");
            caps.SetCapability("version", "latest");
            caps.SetCapability("username", sauceUserName);
            caps.SetCapability("accessKey", sauceAccessKey);

            caps.SetCapability("name", MethodBase.GetCurrentMethod().Name);
            caps.SetCapability("build", "SampleReleaseA");
            var tags = new List<string> { "Release1", "SmokeTests", "LoginFeature" };
            caps.SetCapability("tags", tags);
            caps.SetCapability("extendedDebugging", true);
            _driver = new RemoteWebDriver(new Uri("https://ondemand.us-west-1.saucelabs.com/wd/hub"), caps,
                TimeSpan.FromSeconds(60));
            _driver.Navigate().GoToUrl("https://www.google.com");
        }

        [TestCleanup]
        public void CleanUpAfterEveryTestMethod()
        {
            var passed = TestContext.CurrentTestOutcome == UnitTestOutcome.Passed;
            ((IJavaScriptExecutor)_driver).ExecuteScript("sauce:context=" + "Stop Test");
            ((IJavaScriptExecutor)_driver).ExecuteScript("sauce:job-result=" + (passed ? "passed" : "failed"));
            _driver?.Quit();
        }
    }
}
