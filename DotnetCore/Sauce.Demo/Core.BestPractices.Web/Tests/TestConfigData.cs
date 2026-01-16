using System.Collections;
using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Safari;

namespace Core.BestPractices.Web.Tests
{
    public class TestConfigData
    {
        private const string defaultBrowserVersion = "latest";
        private const string defaultOS = "Windows 11";

        private static readonly SafariOptions safariOptions = new()
        {
            BrowserVersion = "latest",
            PlatformName = "macOS 10.15"
        };

        private static readonly ChromeOptions chromeOptions = new()
        {
            BrowserVersion = "latest",
            PlatformName = "Windows 11",
        };

        private static readonly EdgeOptions edgeOptions = new()
        {
            BrowserVersion = "latest",
            PlatformName = "Windows 11"
        };

        internal static IEnumerable PopularDesktopCombinations
        {
            get
            {
                yield return new TestFixtureData(safariOptions);
                yield return new TestFixtureData(chromeOptions);
                yield return new TestFixtureData(edgeOptions);
            }
        }


        internal static IEnumerable PopularAndroidSimulators
        {
            get
            {
                yield return new TestFixtureData("Google Pixel 9 Emulator", "16.0");
                yield return new TestFixtureData("Google Pixel 9 Emulator", "16.0");
            }
        }

        internal static IEnumerable PopularIOSSimulators
        {
            get
            {
                yield return new TestFixtureData("iPhone Simulator", "16.2");
            }
        }

        public static IEnumerable MostPopularAndroidDevices
        {
            get
            {
                // Using specific Android versions for real device testing
                yield return new TestFixtureData("Google.*", "14");
                // duplication for massive parallel example
                yield return new TestFixtureData("Samsung.*", "14");
            }
        }

        public static IEnumerable MostPopularIOSDevices
        {
            get
            {
                // Using specific iOS versions for real device testing
                yield return new TestFixtureData("iPhone 14.*", "17");
                yield return new TestFixtureData("iPhone 15.*", "17");
                //duplication only for parallel example
                yield return new TestFixtureData("iPhone 14.*", "17");
                yield return new TestFixtureData("iPhone 15.*", "17");
            }
        }
    }
}