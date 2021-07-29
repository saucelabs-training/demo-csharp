using System;

namespace Core.Common
{
    public class SauceLabsEndpoint
    {
        public static string SauceUsWestDomain = "@ondemand.us-west-1.saucelabs.com/wd/hub";
        public string SauceHubUrl => "https://ondemand.saucelabs.com/wd/hub";
        public Uri SauceHubUri => new(SauceHubUrl);
        public static Uri UsWestHubUri => new($"https://{SauceUsWestDomain}");

        public static string HeadlessSeleniumUrl => "https://ondemand.us-east-1.saucelabs.com/wd/hub";

        public static string HeadlessRestApiUrl => "https://us-east-1.saucelabs.com/rest/v1";

        public Uri EmusimUri(string sauceUser, string sauceKey)
        {
            return new($"https://{sauceUser}:{sauceKey}{SauceUsWestDomain}");
        }
    }
}