using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace Core.Selenium.Examples.Selenium4.NewFeatures
{
    [TestClass]
    public class AttributeProperty
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

        /**
     * Property and Attribute often return the same value (especially in Java, since values are converted to Strings,
     * but they are different
     * attribute is defined in html spec: https://dom.spec.whatwg.org/#concept-element-attribute
     * property is defined in ecma spec: https://262.ecma-international.org/5.1/#sec-4.3.26
     *
     * WebElement#getAttribute guesses which value you want from an element's attribute or property value and returns that
     *
     * Since this doesn't make sense in a specification, w3c defines 2 new endpoints, made available in Selenium as:
     * WebElement#getDomProperty and WebElement#getDomAttribute
     *
     * The old behavior with the existing method is still available, but executes a large javascript blob
     * New behavior should be preferred for performance and preciseness
     */
        [TestMethod]
        public void BooleanTest()
        {
            Driver.Navigate().GoToUrl("http://watir.com/examples/forms_with_input_elements.html");
            var element = Driver.FindElement(By.CssSelector("#new_user_interests_books"));

            Assert.AreEqual("true", element.GetAttribute("checked"));
            Assert.AreEqual("true", element.GetDomAttribute("checked"));
            Assert.AreEqual("True", element.GetDomProperty("checked"));

            element.Click();

            Assert.IsNull(element.GetAttribute("checked"));
            Assert.AreEqual("true", element.GetDomAttribute("checked"));
            Assert.AreEqual("False", element.GetDomProperty("checked"));
        }

        [TestMethod]
        public void StringTest()
        {
            Driver.Navigate().GoToUrl("http://watir.com/examples/forms_with_input_elements.html");
            var element = Driver.FindElement(By.CssSelector("#new_user_occupation"));

            Assert.AreEqual("Developer", element.GetAttribute("value"));
            Assert.AreEqual("Developer", element.GetDomAttribute("value"));
            Assert.AreEqual("Developer", element.GetDomProperty("value"));

            element.Clear();
            element.SendKeys("Engineer");

            Assert.AreEqual("Engineer", element.GetAttribute("value"));
            Assert.AreEqual("Developer", element.GetDomAttribute("value"));
            Assert.AreEqual("Engineer", element.GetDomProperty("value"));
        }

        [TestMethod]
        public void ProcessValuesTest()
        {
            Driver.Navigate().GoToUrl("http://watir.com/examples/non_control_elements.html");
            var element = Driver.FindElement(By.CssSelector("#link_3"));

            Assert.AreEqual("http://watir.com/examples/forms_with_input_elements.html", element.GetAttribute("href"));
            Assert.AreEqual("forms_with_input_elements.html", element.GetDomAttribute("href"));
            Assert.AreEqual("http://watir.com/examples/forms_with_input_elements.html", element.GetDomProperty("href"));
        }
        
        [TestMethod]
        public void CaseSensitivityTest()
        {
            Driver.Navigate().GoToUrl("http://watir.com/examples/forms_with_input_elements.html");
            var element = Driver.FindElement(By.CssSelector("#new_user_email"));

            Assert.AreEqual("new_user_email", element.GetAttribute("nAme"));
            Assert.AreEqual("new_user_email", element.GetDomAttribute("nAme"));
            Assert.IsNull(element.GetDomProperty("nAme"));
        }

        [TestMethod]
        public void ClassNameTest()
        {
            Driver.Navigate().GoToUrl("http://watir.com/examples/forms_with_input_elements.html");
            var element = Driver.FindElement(By.CssSelector("#new_user_first_name"));

            Assert.AreEqual("name", element.GetAttribute("className"));
            Assert.IsNull(element.GetDomAttribute("className"));
            Assert.AreEqual("name", element.GetDomProperty("className"));
            
            Assert.AreEqual("name", element.GetAttribute("class"));
            Assert.AreEqual("name", element.GetDomAttribute("class"));
            Assert.IsNull(element.GetDomProperty("class"));
        }

        [TestCleanup]
        public void Teardown()
        {
            var isPassed = TestContext.CurrentTestOutcome == UnitTestOutcome.Passed;
            var script = "sauce:job-result=" + (isPassed ? "passed" : "failed");
            ((IJavaScriptExecutor)Driver).ExecuteScript(script);

            Driver?.Quit();
        }
    }
}