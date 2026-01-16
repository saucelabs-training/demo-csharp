using System;

namespace Core.Common
{
    public class SauceLabsEndpoint
    {
        public static string SauceUsWestDomain = "@ondemand.us-west-1.saucelabs.com/wd/hub";
        public string SauceHubUrl => "https://ondemand.us-west-1.saucelabs.com/wd/hub";
        public Uri SauceHubUri => new(SauceHubUrl);
        public static Uri UsWestHubUri => new($"https://{SauceUsWestDomain}");

        public Uri SauceUri(string sauceUser, string sauceKey)
        {
            return new($"https://{sauceUser}:{sauceKey}{SauceUsWestDomain}");
        }
    }
}