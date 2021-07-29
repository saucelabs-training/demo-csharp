using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;

namespace Core.Selenium.Examples.RDC.Web
{
    public class MobileBaseTest : AllTestsBase
    {
        [SetUp]
        public void MobileBaseSetup()
        {
            MobileOptions = new AppiumOptions();
            MobileOptions.AddAdditionalCapability(MobileCapabilityType.DeviceName, DeviceName);
            MobileOptions.AddAdditionalCapability(MobileCapabilityType.PlatformName, Platform);
            MobileOptions.AddAdditionalCapability(MobileCapabilityType.BrowserName, Browser);
            MobileOptions.AddAdditionalCapability("name", TestContext.CurrentContext.Test.Name);
            MobileOptions.AddAdditionalCapability("newCommandTimeout", 90);
            MobileOptions.AddAdditionalCapability("build", Common.Constants.BuildId);
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