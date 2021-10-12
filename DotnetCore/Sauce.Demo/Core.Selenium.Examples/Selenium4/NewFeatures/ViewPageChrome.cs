using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace Core.Selenium.Examples.Selenium4.NewFeatures
{
    [TestClass]
    public class ViewPageChrome
    {
        public IWebDriver Driver { get; set; }

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void Setup()
        {
            var browserOptions = new ChromeOptions();
            browserOptions.PlatformName = "Windows 10";
            browserOptions.BrowserVersion = "latest";
            browserOptions.AddArguments("--headless");

            var sauceOptions = new Dictionary<string, object>();
            sauceOptions.Add("name", TestContext.TestName);
            sauceOptions.Add("username", Environment.GetEnvironmentVariable("SAUCE_USERNAME"));
            sauceOptions.Add("accessKey", Environment.GetEnvironmentVariable("SAUCE_ACCESS_KEY"));

            browserOptions.AddAdditionalOption("sauce:options", sauceOptions);
            var sauceUrl = new Uri("https://ondemand.us-west-1.saucelabs.com/wd/hub");

            Driver = new RemoteWebDriver(sauceUrl, browserOptions);
        }

        [TestMethod]
        public void ScreenshotTest()
        {
            Driver.Navigate().GoToUrl("https://www.saucedemo.com/v1/inventory.html");
            var parentFullName = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName;

            
            ((ITakesScreenshot) Driver).GetScreenshot().SaveAsFile(parentFullName + "/Selenium4/Resources/ChromeScreenshot.png", 
                ScreenshotImageFormat.Png);
        }

        [TestMethod]
        public void PrintPageTest()
        {
            Driver.Navigate().GoToUrl("https://www.saucedemo.com/v1/inventory.html");
            var parentFullName = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName;

            ((ISupportsPrint) Driver).Print(new PrintOptions()).SaveAsFile(parentFullName + "/Selenium4/Resources/ChromePrintPage.pdf");
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