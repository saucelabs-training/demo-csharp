using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Globalization;
using Common.SauceLabs;

namespace Common
{
    public class WebDriverFactory
    {
        private SauceLabsCapabilities _sauceCustomCapabilities;
        private Dictionary<string, object> _capabilities;
        private string sauceHubUrl = new SauceLabsEndpoint().SauceHubUrl;

        public string SeleniumHubUrl
        {
            get
            {
                return sauceHubUrl;
            }
            set
            {
                sauceHubUrl = value;
            }
        }

        public WebDriverFactory()
        {
            _sauceCustomCapabilities = new SauceLabsCapabilities();
            _capabilities = new Dictionary<string, object>();
        }

        public WebDriverFactory(SauceLabsCapabilities sauceConfig)
        {
            _sauceCustomCapabilities = sauceConfig;
            _capabilities = new Dictionary<string, object>();
        }
        public IWebDriver CreateSauceDriver(string testCaseName)
        {
            SetVMCapabilities("safari", "latest", "mac 10.13");
            return SetSauceCapabilities(testCaseName, _capabilities);
        }
        public IWebDriver CreateSauceDriver(string browser, string browserVersion, string osPlatform)
        {
            return CreateSauceDriver(browser, browserVersion, osPlatform, _sauceCustomCapabilities);
        }
        public RemoteWebDriver CreateSauceDriver(
            string browser, string browserVersion, string osPlatform, SauceLabsCapabilities sauceConfiguration)
        {
            var userName = SauceUser.Name;
            var accessKey = SauceUser.AccessKey;
            if (sauceConfiguration.IsHeadless)
            {
                SeleniumHubUrl = new SauceLabsEndpoint().HeadlessSeleniumUrl;
            }
            _capabilities = new Dictionary<string, object>();
            SetUserAndKey(userName, accessKey);
            SetVMCapabilities(browser, browserVersion, osPlatform);
            //an important flag to set for Edge and possibly Safari
            _capabilities["avoidProxy"] = true;
            SetDebuggingCapabilities(_capabilities);
            _capabilities["build"] = SauceLabsCapabilities.BuildName;
            //_capabilities["tunnelIdentifier"] = "NikolaysTunnel";
            return GetSauceRemoteDriver();
        }
        private void SetUserAndKey(string userName, string accessKey)
        {
            _capabilities["username"] = userName;
            _capabilities["accessKey"] = accessKey;
        }
        private void SetVMCapabilities(string browser, string browserVersion, string osPlatform)
        {
            _capabilities["browserName"] = browser;
            _capabilities["browserVersion"] = browserVersion;
            _capabilities["platformName"] = osPlatform;
        }

        private RemoteWebDriver GetSauceRemoteDriver()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.PlatformName = _capabilities.ContainsKey("platformName") ? _capabilities["platformName"].ToString() : "";
            chromeOptions.BrowserVersion = _capabilities.ContainsKey("browserVersion") ? _capabilities["browserVersion"].ToString() : "";
            
            // Add all sauce capabilities
            var sauceOptions = new Dictionary<string, object>();
            foreach (var capability in _capabilities)
            {
                if (capability.Key != "browserName" && capability.Key != "browserVersion" && capability.Key != "platformName")
                {
                    sauceOptions[capability.Key] = capability.Value;
                }
            }
            chromeOptions.AddAdditionalOption("sauce:options", sauceOptions);
            
            return new RemoteWebDriver(new Uri(SeleniumHubUrl),
                chromeOptions.ToCapabilities(), TimeSpan.FromSeconds(600));
        }
        private IWebDriver SetSauceCapabilities(string testCaseName, Dictionary<string, object> capabilities)
        {
            _capabilities = capabilities;
            SetUserAndKey(SauceUser.Name, SauceUser.AccessKey);

            //CUSTOM SAUCE CAPABILITIES
            //These capabilities are excellent for debugging and make it much easier.
            //However, if your tests are pretty stable and you want faster tests, disable all the debugging features
            //capabilities["extendedDebugging"] = true;
            //capabilities["recordVideo"] = false;
            //capabilities["videoUploadOnPass"] = false;
            //capabilities["recordScreenshots"] = false;
            _capabilities["build"] = $"SauceExamples-{DateTime.Now.ToString(CultureInfo.InvariantCulture)}";
            var tags = new List<string> { "withDebugging", "automationGroupName1", "automationGroupName2" };
            _capabilities["tags"] = tags;
            //capabilities["tunnelIdentifier"] = "NikolaysTunnel";

            //SAUCE TIMEOUT CAPABILITIES
            SetSauceTimeouts();
            var driver = GetSauceRemoteDriver();
            new SauceJavaScriptExecutor(driver).SetTestName(testCaseName);
            return driver;
        }


        private void SetSauceTimeouts()
        {
            //How long is a test allowed to run?
            _capabilities["maxDuration"] = 3600;
            //Selenium crash might hang a command, this is the max time allowed to wait for a Selenium command
            //Keep this low, no reason to wait around a long time for a hanging command to fail
            _capabilities["commandTimeout"] = 60;
            //How long can the browser wait before a new command?
            _capabilities["idleTimeout"] = 1000;
        }

        private void SetDebuggingCapabilities(Dictionary<string, object> capabilities)
        {
            //These capabilities are excellent for debugging and make it much easier.
            //However, if your tests are pretty stable and you want faster tests, disable all the debugging features
            if (_sauceCustomCapabilities.IsDebuggingEnabled)
            {
                SetDebuggingForHeadless(_sauceCustomCapabilities.IsHeadless, capabilities);
                SetDebuggingForNonHeadless(_sauceCustomCapabilities.IsHeadless, capabilities);
                _sauceCustomCapabilities.Tags.Add("withDebuggingEnabled");
                return;
            }

            capabilities["extendedDebugging"] = false;
            capabilities["recordVideo"] = true;
            capabilities["videoUploadOnPass"] = true;
            capabilities["recordScreenshots"] = true;
            _sauceCustomCapabilities.Tags.Add("withDebuggingDisabled");
        }

        private void SetDebuggingForNonHeadless(bool isHeadless, Dictionary<string, object> capabilities)
        {
            if (isHeadless) return;
            capabilities["extendedDebugging"] = true;
            capabilities["recordVideo"] = true;
            capabilities["videoUploadOnPass"] = true;
            capabilities["recordScreenshots"] = true;
        }

        private void SetDebuggingForHeadless(bool isHeadless, Dictionary<string, object> capabilities)
        {
            if (!isHeadless)
                return;
            capabilities["recordScreenshots"] = true;
        }
    }
}
