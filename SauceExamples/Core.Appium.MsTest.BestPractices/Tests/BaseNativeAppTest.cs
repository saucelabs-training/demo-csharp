using System;

namespace Core.Appium.Nunit.BestPractices.Tests
{
    public class BaseNativeAppTest
    {
        public static string HubUrlPart => "ondemand.us-west-1.saucelabs.com/wd/hub";
        public string SauceUser => Environment.GetEnvironmentVariable("SAUCE_USERNAME");

        public string SauceAccessKey =>
            Environment.GetEnvironmentVariable("SAUCE_ACCESS_KEY");

        public string Url => $"https://{SauceUser}:{SauceAccessKey}@{HubUrlPart}";
    }
}