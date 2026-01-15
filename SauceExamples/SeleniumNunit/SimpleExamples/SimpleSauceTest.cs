using System;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace Selenium3.Nunit.Scripts.SimpleExamples
{
    [TestFixture]
    [Category("SimpleTest")]
    public class SimpleSauceTest
    {
        IWebDriver _driver;
        [Test]
        public void SimpleTest()
        {
            //TODO please supply your Sauce Labs user name in an environment variable
            var sauceUserName = Environment.GetEnvironmentVariable(
                "SAUCE_USERNAME", EnvironmentVariableTarget.User);
            //TODO please supply your own Sauce Labs access Key in an environment variable
            var sauceAccessKey = Environment.GetEnvironmentVariable(
                "SAUCE_ACCESS_KEY", EnvironmentVariableTarget.User);

            ChromeOptions options = new ChromeOptions();
            options.AddAdditionalCapability(CapabilityType.Version, "latest", true);
            options.AddAdditionalCapability(CapabilityType.Platform, "Windows 10", true);
            options.AddAdditionalCapability("username", sauceUserName, true);
            options.AddAdditionalCapability("accessKey", sauceAccessKey, true);
            options.AddAdditionalCapability("name", TestContext.CurrentContext.Test.Name, true);
            options.AddAdditionalCapability("build", "ShwabTeamName:" + DateTime.Now, true);


            _driver = new RemoteWebDriver(new Uri("https://ondemand.us-west-1.saucelabs.com/wd/hub"), options.ToCapabilities(),
                TimeSpan.FromSeconds(600));
            _driver.Navigate().GoToUrl("https://www.google.com");
            Assert.Pass();
        }

        [TearDown]
        public void CleanUpAfterEveryTestMethod()
        {
            var passed = TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed;
            ((IJavaScriptExecutor)_driver).ExecuteScript("sauce:job-result=" + (passed ? "passed" : "failed"));
            _driver?.Quit();
        }
    }
}
