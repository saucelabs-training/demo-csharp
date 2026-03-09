using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;

namespace Core.BestPractices.Web.Tests.Mobile.Android
{
    /// <summary>
    /// Demonstrates the Sauce Labs Real Device Access API:
    /// 1. Reserve a device session via the REST API
    /// 2. Start a persistent Appium server for that session
    /// 3. Run multiple tests back-to-back on the SAME device
    /// 4. Release the device session when all tests are done
    ///
    /// </summary>
    [TestFixture]
    [NonParallelizable]
    public class AccessApiDemoAppiumTest
    {
        private static string _sessionId = string.Empty;  // The ID for our reserved device session
        private static Uri _appiumUrl;     // The Appium URL, reused for all tests

        private static readonly string BaseApiUrl = "https://api.us-west-1.saucelabs.com";
        private static readonly string SauceUsername = Environment.GetEnvironmentVariable("SAUCE_USERNAME") ?? "username";
        private static readonly string SauceAccessKey = Environment.GetEnvironmentVariable("SAUCE_ACCESS_KEY") ?? "access_key";

        private static readonly HttpClient HttpClient = new()
        {
            Timeout = TimeSpan.FromSeconds(20)
        };

        /// <summary>
        /// Reserves a device via the Sauce Labs Access API and starts a persistent
        /// Appium server for that device. Runs once before any [Test] methods.
        /// </summary>
        [OneTimeSetUp]
        public void SetupSuite()
        {
            var createSessionBody = JsonSerializer.Serialize(new
            {
                device = new { os = "android" }
            });

            var createSessionResponse = Post("/rdc/v2/sessions", createSessionBody);
            _sessionId = createSessionResponse.GetProperty("id").GetString() ?? string.Empty;

            WaitForSessionToBeActive(_sessionId);

            _appiumUrl = StartAppiumServer(_sessionId);
        }

        /// <summary>
        /// The final cleanup. Releases the device session after all tests are done.
        /// </summary>
        [OneTimeTearDown]
        public void TearDownSuite()
        {
            if (string.IsNullOrEmpty(_sessionId)) return;
            Console.WriteLine("Releasing Device Session: " + _sessionId);
            _ = Delete("/rdc/v2/sessions/" + _sessionId);
        }

        // Each of these methods runs back-to-back on the SAME device.

        private AndroidDriver _testDriver;

        /// <summary>
        /// Quits the Appium client connection after each test.
        /// The underlying device session remains active for the next test.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            _testDriver?.Quit();
            _testDriver = null;
        }

        [Test]
        public void FirstTest()
        {
            var appiumOptions = new AppiumOptions();
            // With Access API, the appium:newCommandTimeout can be longer than the Sauce max of 90s.
            appiumOptions.AddAdditionalAppiumOption(MobileCapabilityType.NewCommandTimeout, 180);
            _testDriver = new AndroidDriver(_appiumUrl, appiumOptions);
            _testDriver.Navigate().GoToUrl("https://www.saucelabs.com");
            Thread.Sleep(TimeSpan.FromSeconds(160));
            _testDriver.Navigate().GoToUrl("https://opensource.saucelabs.com");
        }

        [Test]
        public void SecondTest()
        {
            _testDriver = new AndroidDriver(_appiumUrl, new AppiumOptions());
            _testDriver.Navigate().GoToUrl("https://www.youtube.com");
        }

        // --- Helper Methods ---

        private static Uri StartAppiumServer(string sessionId)
        {
            var requestBody = JsonSerializer.Serialize(new { appiumVersion = "latest" });
            var response = Post($"/rdc/v2/sessions/{sessionId}/appiumserver", requestBody);

            var appiumUrlString = response.GetProperty("url").GetString();
            if (string.IsNullOrEmpty(appiumUrlString))
                throw new Exception("Appium server URL was not returned by the API.");

            Console.WriteLine("Appium server URL: " + appiumUrlString);
            return new Uri(appiumUrlString);
        }

        private void WaitForSessionToBeActive(string sessionId)
        {
            Console.WriteLine("Waiting for session active...");
            for (int i = 0; i < 5; i++)
            {
                var getSessionResponse = Get("/rdc/v2/sessions/" + sessionId);
                var currentState = getSessionResponse.GetProperty("state").GetString();
                Console.WriteLine($"Current state: {currentState} Session info: {getSessionResponse}");

                if ("ACTIVE".Equals(currentState, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Session is now ACTIVE. Exiting loop.");
                    return;
                }

                Console.WriteLine("Waiting for 5 seconds...");
                Thread.Sleep(5000);
            }

            throw new Exception("Session did not become active");
        }

        private static JsonElement Post(string apiPath, string requestBody)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, BaseApiUrl + apiPath);
            AddAuthHeader(request);
            request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            var response = HttpClient.SendAsync(request).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return JsonSerializer.Deserialize<JsonElement>(responseBody);
        }

        private static JsonElement Get(string apiPath)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, BaseApiUrl + apiPath);
            AddAuthHeader(request);

            var response = HttpClient.SendAsync(request).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return JsonSerializer.Deserialize<JsonElement>(responseBody);
        }

        private static JsonElement Delete(string apiPath)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, BaseApiUrl + apiPath);
            AddAuthHeader(request);

            var response = HttpClient.SendAsync(request).GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return JsonSerializer.Deserialize<JsonElement>(responseBody);
        }

        private static void AddAuthHeader(HttpRequestMessage request)
        {
            var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{SauceUsername}:{SauceAccessKey}"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);
        }
    }
}

