using System;
using OpenQA.Selenium;
using RestSharp;
using RestSharp.Authenticators;

namespace Common.SauceLabs
{
    public class SauceAPI
    {
        public void UpdateTestStatus(bool isTestPassed, SessionId sessionId)
        {
            //API Docs: https://wiki.saucelabs.com/display/DOCS/Job+Methods#JobMethods-UpdateJob

            var options = new RestClientOptions("https://saucelabs.com/rest/v1")
            {
                Authenticator = new HttpBasicAuthenticator(SauceUser.Name, SauceUser.AccessKey)
            };
            var client = new RestClient(options);
            var request = new RestRequest($"/{SauceUser.Name}/jobs/{sessionId}")
                .AddJsonBody(new { passed = isTestPassed });
            var response = client.ExecutePut(request);
        }
    }
}