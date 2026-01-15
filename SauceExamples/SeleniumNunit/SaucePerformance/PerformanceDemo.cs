using System;
using System.Collections.Generic;
using Common.SauceLabs;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace Selenium3.Nunit.Scripts.SaucePerformance
{
    [TestFixture]
    [Category("performance")]
    public class PerformanceDemo
    {
        public RemoteWebDriver Driver { get; set; }

        [SetUp]
        public void RunBeforeEveryTest()
        {
            var sauceUserName = Environment.GetEnvironmentVariable("SAUCE_USERNAME", EnvironmentVariableTarget.User);
            var sauceAccessKey = Environment.GetEnvironmentVariable("SAUCE_ACCESS_KEY", EnvironmentVariableTarget.User);

            var chromeOptions = new ChromeOptions()
            {
                BrowserVersion = "latest",
                PlatformName = "Windows 10",
            };
            var sauceOptions = new Dictionary<string, object>
            {
                ["username"] = sauceUserName,
                ["accessKey"] = sauceAccessKey,
                ["name"] = TestContext.CurrentContext.Test.Name,
                ["extendedDebugging"] = true,
                ["capturePerformance"] = true
            };
            chromeOptions.AddAdditionalCapability("sauce:options", sauceOptions, true);

            Driver = new RemoteWebDriver(new Uri("https://ondemand.us-west-1.saucelabs.com/wd/hub"),
                chromeOptions.ToCapabilities(), TimeSpan.FromSeconds(600));
        }

        [TearDown]
        public void CleanUp()
        {
            var isPassed = TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Passed;
            new SauceJavaScriptExecutor(Driver).LogTestStatus(isPassed);
            Driver.Quit();
        }
        [Test]

        public void W3CPerformanceTestForSauceDemo()
        {
            Driver.Navigate().GoToUrl("https://www.saucedemo.com");
        }

        /*
         * A simple test that will capture the performance of this page
         * based on previous execution and history. This execution
         * estabilished a baseline that will makee sure that our test doesn't
         * deviate away from it.
         */
        [Test]
        public void W3CPerformanceTestForUltimateQa()
        {
            Driver.Navigate().GoToUrl("https://www.ultimateqa.com");
        }

        /*
         * This test makes sure that the load time of the website is
         * within a range that we specified. In this case, wee are
         * allowing a 20% variation in our page load speed
         */
        [Test]
        public void SauceDemoLoadTimeShouldBeWithin20Percent()
        {
            Driver.Navigate().GoToUrl("https://www.saucedemo.com");

            var metrics = new Dictionary<string, object>
            {
                ["type"] = "sauce:performance"
            };
            var performanceMetrics = (Dictionary<string, object>)((IJavaScriptExecutor)Driver).ExecuteScript("sauce:log", metrics);
            Assert.That(performanceMetrics["load"], Is.EqualTo(450).Within(20).Percent);
        }

        /*
         * Make sure that the web page speed index score is within a certain range.
         * The speed index is a score from 0 to infinity.
         * In this test we are making sure that the score is within 20 %
         *
         */
        [Test]
        public void SauceDemoSpeedIndexShouldBeWithin20Percent()
        {
            Driver.Navigate().GoToUrl("https://www.saucedemo.com");

            var metrics = new Dictionary<string, object>
            {
                ["type"] = "sauce:performance"
            };
            var performanceMetrics = (Dictionary<string, object>)((IJavaScriptExecutor)Driver).ExecuteScript("sauce:log", metrics);
            Assert.That(performanceMetrics["speedIndex"], Is.EqualTo(415).Within(20).Percent);
        }

        [Test]
        public void MultiPagePerformaceTest()
        {
            Driver.Navigate().GoToUrl("https://www.ultimateqa.com");
            Driver.Navigate().GoToUrl("https://www.ultimateqa.com/selenium-java");
            Driver.Navigate().GoToUrl("https://ultimateqa.com/selenium-java-2/");

            var metrics = new Dictionary<string, object>
            {
                ["name"] = TestContext.CurrentContext.Test.Name,
                ["metrics"] = "load"
            };
            var perfResult = (Dictionary<string, object>)((IJavaScriptExecutor)Driver).ExecuteScript("sauce:performance", metrics);
            Assert.That(perfResult["result"], Is.EqualTo("pass"));
        }
    }
}
