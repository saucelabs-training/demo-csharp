using System;
using System.Collections.Generic;
using Common.SauceLabs;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace Selenium3.Nunit.Scripts.SaucePerformance
{
    [TestFixture]
    [Category("performance")]
    public class CustomCapabilitiesTests
    {
        public RemoteWebDriver Driver { get; set; }

        [Test]
        public void NetworkThrottleOffline()
        {
            var throttleCondition = new Dictionary<string, object>
            {
                ["condition"] = "offline"
            };
            ((IJavaScriptExecutor)Driver).ExecuteScript("sauce:throttleNetwork", throttleCondition);
            Driver.Navigate().GoToUrl("http://www.saucedemo.com");
            Assert.Throws<WebDriverTimeoutException>(() => isDisplayed());
        }

        private bool isDisplayed()
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(5));
            return wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("btn_action"))).Displayed;
        }

        [Test]
        public void NetworkThrottle3G()
        {
            var throttleCondition = new Dictionary<string, object>
            {
                ["condition"] = "Regular 3G"
            };
            ((IJavaScriptExecutor)Driver).ExecuteScript("sauce:throttleNetwork", throttleCondition);
            Driver.Navigate().GoToUrl("http://www.saucedemo.com");
        }
        [Test]
        [Description("Broken, there's a bug")]
        public void ReplaceImages()
        {
            Driver.Navigate().GoToUrl("http://www.wswebcreation.nl/");
            var sauceInterceptConditions = new Dictionary<string, object>
            {
                ["url"] = "**/*.jpg",
                ["redirect"] = "https://www.tvvantoen.nl/wp-content/themes/website/data/php/timthumb.php?src=https://www.tvvantoen.nl/wp-content/uploads/the-a-team-poster.jpg&w=700&q=100"
            };
            ((IJavaScriptExecutor)Driver).ExecuteScript("sauce:intercept", sauceInterceptConditions);
            Driver.Navigate().GoToUrl("http://www.wswebcreation.nl/");
        }
        [Test]
        public void ReplaceSingleImage()
        {
            Driver.Navigate().GoToUrl("https://www.saucedemo.com/inventory.html");
            var sauceInterceptConditions = new Dictionary<string, object>
            {
                ["url"] = "https://www.saucedemo.com/img/sauce-backpack-1200x1500.jpg",
                ["redirect"] = "https://i.pinimg.com/originals/be/82/15/be821544fc5f328567cb538f96edb49a.jpg"
            };
            ((IJavaScriptExecutor)Driver).ExecuteScript("sauce:intercept", sauceInterceptConditions);
            Driver.Navigate().GoToUrl("https://www.saucedemo.com/inventory.html");
        }
        [Test]
        public void GetSauceMetrics()
        {
            //https://wiki.saucelabs.com/display/DOCS/Custom+Sauce+Labs+WebDriver+Extensions+for+Network+and+Log+Commands
            //this works
            var performanceMetrics = new Dictionary<string, object>();
            performanceMetrics["type"] = "sauce:metrics";
            var output = ((IJavaScriptExecutor)Driver).ExecuteScript("sauce:log", performanceMetrics);
            ((IJavaScriptExecutor)Driver).ExecuteScript("sauce:network");
        }

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
                ["capturePerformance"] = true,
                ["crmuxdriverVersion"] = "beta"
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


    }
}
