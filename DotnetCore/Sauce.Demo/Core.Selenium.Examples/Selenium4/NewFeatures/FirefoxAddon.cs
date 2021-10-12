using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Castle.Core.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;

namespace Core.Selenium.Examples.Selenium4.NewFeatures
{
    [TestClass]
    public class FirefoxAddon
    {
        public IWebDriver Driver { get; set; }

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void Setup()
        {
            var browserOptions = new FirefoxOptions();
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
        public void AddRemoveExtension()
        {
            IHasCommandExecutor hasCommandExecutor = Driver as IHasCommandExecutor;
            var installAddonCommandInfo =
                new HttpCommandInfo(HttpCommandInfo.PostCommand, "/session/{sessionId}/moz/addon/install");
            var uninstallAddonCommandInfo =
                new HttpCommandInfo(HttpCommandInfo.PostCommand, "/session/{sessionId}/moz/addon/uninstall");
            hasCommandExecutor.CommandExecutor.TryAddCommand("installAddon", installAddonCommandInfo);
            hasCommandExecutor.CommandExecutor.TryAddCommand("uninstallAddon", uninstallAddonCommandInfo);

            var parentFullName = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName;
            var localFile = parentFullName + "/Selenium4/Resources/ninja_saucebot-1.0-an+fx.xpi";

            // This duplicates Upload File functionality
            string base64zip = string.Empty;
            using (MemoryStream fileUploadMemoryStream = new MemoryStream())
            {
                using (ZipArchive zipArchive = new ZipArchive(fileUploadMemoryStream, ZipArchiveMode.Create))
                {
                    string fileName = Path.GetFileName(localFile);
                    zipArchive.CreateEntryFromFile(localFile, fileName);
                }

                base64zip = Convert.ToBase64String(fileUploadMemoryStream.ToArray());
            }

            Dictionary<string, object> uploadParams = new Dictionary<string, object>();
            uploadParams.Add("file", base64zip);

            SessionId sessionId = ((RemoteWebDriver)Driver).SessionId;

            var uploadCommand = new Command(sessionId, "uploadFile", uploadParams);

            Response response = hasCommandExecutor.CommandExecutor.Execute(uploadCommand);

            Dictionary<string, object> addonParams = new Dictionary<string, object>();
            addonParams.Add("path", response.Value.ToString());
            addonParams.Add("temporary", false);

            var installAddonCommand = new Command(sessionId, "installAddon", addonParams);
            String id = hasCommandExecutor.CommandExecutor.Execute(installAddonCommand).Value.ToString();

            Driver.Navigate().GoToUrl("https://www.saucedemo.com");

            Assert.IsFalse(Driver.FindElements(By.CssSelector(".bot_column2")).IsNullOrEmpty());
            
            Dictionary<string, object> removeParams = new Dictionary<string, object>();
            removeParams.Add("id", id);

            var uninstallAddonCommand = new Command(sessionId, "uninstallAddon", removeParams);
            hasCommandExecutor.CommandExecutor.Execute(uninstallAddonCommand);

            Driver.Navigate().Refresh();

            Assert.IsTrue(Driver.FindElements(By.CssSelector(".bot_column2")).IsNullOrEmpty());
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