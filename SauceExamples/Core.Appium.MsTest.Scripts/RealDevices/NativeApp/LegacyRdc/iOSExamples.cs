﻿using System;
using Common.SauceLabs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace Core.Appium.Examples.RealDevices.NativeApp.LegacyRdc
{
    [TestClass]
    public class IOSExamples
    {
        private static string RdcUsHubUrl => "https://us1.appium.testobject.com/wd/hub";

        public const string APPIUM_VERSION = "1.16.0";

        private IOSDriver _driver;
        private SessionId _sessionId;
        public TestContext TestContext { get; set; }


        [TestCleanup]
        public void Teardown()
        {
            if (_driver == null) return;

            _sessionId = _driver.SessionId;
            _driver.Quit();
            new SimpleSauce().Rdc.UpdateTestStatus(GetTestStatus(), _sessionId);
        }

        [TestMethod]
        [TestCategory("Rdc")]
        [TestCategory("NativeApp")]
        [TestCategory("iOS")]
        public void AnyIPhone()
        {
            var capabilities = new AppiumOptions();
            //We can run on any iPhone Device
            capabilities.AddAdditionalCapability(MobileCapabilityType.DeviceName, "iPhone.*");
            capabilities.AddAdditionalCapability(MobileCapabilityType.PlatformName, "iOS");
            //TODO it's a best practice to set the appium version so that you're always getting the latest
            capabilities.AddAdditionalCapability("appiumVersion", APPIUM_VERSION);
            /*
             * !!!!!!
             * TODO first you must upload an app to RDC so that you get your app key
             * Then, make sure you can hardcode it here just to get started
             */
            capabilities.AddAdditionalCapability("testobject_api_key", new ApiKeys().Rdc.Apps.SampleAppIOS);
            capabilities.AddAdditionalCapability("name", TestContext.TestName);
            capabilities.AddAdditionalCapability("newCommandTimeout", 90);

            //60 seconds for the connection timeout
            _driver = new IOSDriver<IOSElement>(new Uri(RdcUsHubUrl), capabilities);
            Assert.IsTrue(IsLoginButtonDisplayed());
        }

        [TestMethod]
        [TestCategory("Rdc")]
        [TestCategory("NativeApp")]
        [TestCategory("iOS")]
        public void IOS13()
        {
            var capabilities = new AppiumOptions();
            //We can run on any iPhone Device
            capabilities.AddAdditionalCapability(MobileCapabilityType.PlatformVersion, "13");
            capabilities.AddAdditionalCapability(MobileCapabilityType.PlatformName, "iOS");
            //TODO it's a best practice to set the appium version so that you're always getting the latest
            capabilities.AddAdditionalCapability("appiumVersion", APPIUM_VERSION);
            /*
             * !!!!!!
             * TODO first you must upload an app to RDC so that you get your app key
             * Then, make sure you can hardcode it here just to get started
             */
            capabilities.AddAdditionalCapability("testobject_api_key", new ApiKeys().Rdc.Apps.SampleAppIOS);
            capabilities.AddAdditionalCapability("name", TestContext.TestName);
            capabilities.AddAdditionalCapability("newCommandTimeout", 90);

            //60 seconds for the connection timeout
            _driver = new IOSDriver<IOSElement>(new Uri(RdcUsHubUrl), capabilities);
            Assert.IsTrue(IsLoginButtonDisplayed());
        }

        private bool IsLoginButtonDisplayed()
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));
            var loginButton = wait.Until(ExpectedConditions.ElementIsVisible(MobileBy.AccessibilityId("test-LOGIN")));
            return loginButton.Displayed;
        }
        private bool GetTestStatus()
        {
            return TestContext.CurrentTestOutcome == UnitTestOutcome.Passed;
        }
    }
}
