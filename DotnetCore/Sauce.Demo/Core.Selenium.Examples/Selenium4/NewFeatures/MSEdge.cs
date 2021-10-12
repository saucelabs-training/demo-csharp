using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace Core.Selenium.Examples.Selenium4.NewFeatures
{
    [TestClass]
    public class MSEdge
    {
        public IWebDriver Driver { get; set; }

        public TestContext TestContext { get; set; }

        [TestMethod]
        public void EdgeOptionsTest()
        {
            var browserOptions = new EdgeOptions();
            browserOptions.BinaryLocation = "/Path/To/Binary";
            browserOptions.AddArguments("foo");
            browserOptions.AddUserProfilePreference("foo", "bar");

            Dictionary<String, Object> caps = (Dictionary<string, object>)browserOptions
                .ToCapabilities()
                .GetCapability("ms:edgeOptions");

            List<String> args = new List<string>() { "foo" };
            CollectionAssert.AreEquivalent(args, (ICollection)caps.GetValueOrDefault("args"));
            
            Assert.AreEqual("bar", ((Dictionary<string, object>)caps.GetValueOrDefault("prefs"))!.GetValueOrDefault("foo"));
            Assert.AreEqual("/Path/To/Binary", caps.GetValueOrDefault("binary"));
        }
    }
}