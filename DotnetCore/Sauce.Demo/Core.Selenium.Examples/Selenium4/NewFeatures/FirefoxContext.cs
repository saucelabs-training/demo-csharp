using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.Extensions;

namespace Core.Selenium.Examples.Selenium4.NewFeatures
{
    [TestClass]
    public class FirefoxContext
    {
        public IWebDriver Driver { get; set; }

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void Setup()
        {
            var browserOptions = new FirefoxOptions();
            browserOptions.SetPreference("intl.accept_languages", "de-DE");

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
        public void ChangePrefs()
        {
            IHasCommandExecutor hasCommandExecutor = Driver as IHasCommandExecutor;
            var getContextCommandInfo = new HttpCommandInfo(HttpCommandInfo.GetCommand, "/session/{sessionId}/moz/context");
            var setContextCommandInfo = new HttpCommandInfo(HttpCommandInfo.PostCommand, "/session/{sessionId}/moz/context");
            hasCommandExecutor.CommandExecutor.TryAddCommand("getContext", getContextCommandInfo);
            hasCommandExecutor.CommandExecutor.TryAddCommand("setContext", setContextCommandInfo);

            SessionId sessionId = ((RemoteWebDriver)Driver).SessionId;
            
            Driver.Navigate().GoToUrl("https://www.google.com");
            var element = Driver.FindElement(By.CssSelector("#gws-output-pages-elements-homepage_additional_languages__als"));
            
            Assert.IsTrue(element.Text.Contains("angeboten auf"));
            
            try
            {
                Driver.ExecuteJavaScript("Services.prefs.setStringPref('intl.accept_languages', 'es-ES')");
                Assert.Fail("Can not change Service prefs in content context, so previous method should fail");
            }
            catch (WebDriverException)
            {
                // This is expected
            }

            var getContextCommand = new Command(sessionId, "getContext", null);
            Response response = hasCommandExecutor.CommandExecutor.Execute(getContextCommand);

            Assert.AreEqual("content", response.Value);

            Dictionary<String, Object> payload = new Dictionary<string, object>();
            payload.Add("context", "chrome");
            var setContextCommand = new Command(sessionId, "setContext", payload);
            hasCommandExecutor.CommandExecutor.Execute(setContextCommand);

            response = hasCommandExecutor.CommandExecutor.Execute(getContextCommand);
            Assert.AreEqual("chrome", response.Value);

            Driver.ExecuteJavaScript("Services.prefs.setStringPref('intl.accept_languages', 'es-ES')");

            try
            {
                Driver.Navigate().Refresh();
                Assert.Fail("Can not navigate in chrome context, so previous method should fail");
            }
            catch (WebDriverException)
            {
                // This is expected
            }

            payload.Remove("context");
            payload.Add("context", "content");
            setContextCommand = new Command(sessionId, "setContext", payload);
            hasCommandExecutor.CommandExecutor.Execute(setContextCommand);

            Driver.Navigate().Refresh();
            element = Driver.FindElement(By.CssSelector("#gws-output-pages-elements-homepage_additional_languages__als"));
            Assert.IsTrue(element.Text.Contains("Ofrecido por"));
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