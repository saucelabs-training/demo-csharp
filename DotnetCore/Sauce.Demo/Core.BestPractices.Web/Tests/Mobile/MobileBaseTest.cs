using Core.Common;
using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;

namespace Core.BestPractices.Web.Tests.Mobile
{
    public class MobileBaseTest : AllTestsBase
    {
        [SetUp]
        public void MobileBaseSetup()
        {
            MobileOptions = new AppiumOptions();
            MobileOptions.AddAdditionalOption(MobileCapabilityType.DeviceName, DeviceName);
            MobileOptions.AddAdditionalOption(MobileCapabilityType.PlatformName, Platform);
            MobileOptions.AddAdditionalOption(MobileCapabilityType.BrowserName, Browser);
            MobileOptions.AddAdditionalOption("name", TestContext.CurrentContext.Test.Name);
            MobileOptions.AddAdditionalOption("newCommandTimeout", 90);
            MobileOptions.AddAdditionalOption("build", Constants.BuildId);
        }

        public readonly string DeviceName;
        public readonly string Platform;
        public readonly string Browser;

        public MobileBaseTest(string deviceName, string platform, string browser)
        {
            DeviceName = deviceName;
            Platform = platform;
            Browser = browser;
        }

        public AppiumOptions MobileOptions { get; set; }
    }
}