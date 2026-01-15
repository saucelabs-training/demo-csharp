using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace Selenium3.Nunit.Scripts.Parallel
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    class ParallelAtMethodsWithSelenium
    {
        private sealed class TestScope : IDisposable
        {
            public IWebDriver Driver { get; }
            private string SauceUserName { get; }
            private string SauceAccessKey { get; }
            private Dictionary<string, object> SauceOptions { get; }
            public TestScope()
            {
                var chromeOptions = new ChromeOptions
                {
                    BrowserVersion = "latest",
                    PlatformName = "Windows 10",
                };
                //TODO please supply your Sauce Labs user name in an environment variable
                SauceUserName = Environment.GetEnvironmentVariable("SAUCE_USERNAME", EnvironmentVariableTarget.User);
                //TODO please supply your own Sauce Labs access Key in an environment variable
                SauceAccessKey = Environment.GetEnvironmentVariable("SAUCE_ACCESS_KEY", EnvironmentVariableTarget.User);
                SauceOptions = new Dictionary<string, object>
                {
                    ["username"] = SauceUserName,
                    ["accessKey"] = SauceAccessKey
                };
                SauceOptions.Add("name", TestContext.CurrentContext.Test.Name);
                chromeOptions.AddAdditionalCapability("sauce:options", SauceOptions, true);

                Driver = new RemoteWebDriver(new Uri("https://ondemand.us-west-1.saucelabs.com/wd/hub"),
                    chromeOptions.ToCapabilities(), TimeSpan.FromSeconds(600));
            }

            public void Dispose()
            {
                //clean-up code goes here
                if (Driver == null)
                    return;
                var passed = TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed;
                //TODO can't get this to work, it always fails

                ((IJavaScriptExecutor)Driver).ExecuteScript("sauce:job-result=" + (passed ? "passed" : "failed"));
                Driver.Quit();
            }
        }
        [Test]
        public void Test1()
        {
            using (var scope = new TestScope())
            {
                scope.Driver.Navigate().GoToUrl("https://www.saucedemo.com");
                Assert.AreEqual("Swag Labs", scope.Driver.Title);
            }
        }
        [Test]
        public void Test2()
        {
            using (var scope = new TestScope())
            {
                scope.Driver.Navigate().GoToUrl("https://www.saucedemo.com");
                Assert.AreEqual("Swag Labs", scope.Driver.Title);
            }
        }
        [Test]
        public void Test4()
        {
            using (var scope = new TestScope())
            {
                scope.Driver.Navigate().GoToUrl("https://www.saucedemo.com");
                Assert.AreEqual("Swag Labs", scope.Driver.Title);
            }
        }
        [Test]
        public void Test5()
        {
            using (var scope = new TestScope())
            {
                scope.Driver.Navigate().GoToUrl("https://www.saucedemo.com");
                Assert.AreEqual("Swag Labs", scope.Driver.Title);
            }
        }
        [Test]
        public void Test6()
        {
            using (var scope = new TestScope())
            {
                scope.Driver.Navigate().GoToUrl("https://www.saucedemo.com");
                Assert.AreEqual("Swag Labs", scope.Driver.Title);
            }
        }
        [Test]
        public void Test7()
        {
            using (var scope = new TestScope())
            {
                scope.Driver.Navigate().GoToUrl("https://www.saucedemo.com");
                Assert.AreEqual("Swag Labs", scope.Driver.Title);
            }
        }
        [Test]
        public void Test8()
        {
            using (var scope = new TestScope())
            {
                scope.Driver.Navigate().GoToUrl("https://www.saucedemo.com");
                Assert.AreEqual("Swag Labs", scope.Driver.Title);
            }
        }
        [Test]
        public void Test9()
        {
            using (var scope = new TestScope())
            {
                scope.Driver.Navigate().GoToUrl("https://www.saucedemo.com");
                Assert.AreEqual("Swag Labs", scope.Driver.Title);
            }
        }
        [Test]
        public void Test10()
        {
            using (var scope = new TestScope())
            {
                scope.Driver.Navigate().GoToUrl("https://www.saucedemo.com");
                Assert.AreEqual("Swag Labs", scope.Driver.Title);
            }
        }
        [Test]
        public void Test11()
        {
            using (var scope = new TestScope())
            {
                scope.Driver.Navigate().GoToUrl("https://www.saucedemo.com");
                Assert.AreEqual("Swag Labs", scope.Driver.Title);
            }
        }
        [Test]
        public void Test12()
        {
            using (var scope = new TestScope())
            {
                scope.Driver.Navigate().GoToUrl("https://www.saucedemo.com");
                Assert.AreEqual("Swag Labs", scope.Driver.Title);
            }
        }
        [Test]
        public void Test13()
        {
            using (var scope = new TestScope())
            {
                scope.Driver.Navigate().GoToUrl("https://www.saucedemo.com");
                Assert.AreEqual("Swag Labs", scope.Driver.Title);
            }
        }
        [Test]
        public void Test14()
        {
            using (var scope = new TestScope())
            {
                scope.Driver.Navigate().GoToUrl("https://www.saucedemo.com");
                Assert.AreEqual("Swag Labs", scope.Driver.Title);
            }
        }
    }
}
