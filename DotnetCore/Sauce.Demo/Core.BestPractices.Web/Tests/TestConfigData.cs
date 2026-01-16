using System.Collections;
using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Safari;

namespace Core.BestPractices.Web.Tests
{
    public class TestConfigData
    {
        private const string defaultBrowserVersion = "";
        private const string defaultOS = "";

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
                yield return new TestFixtureData("latest", "macOS 10.15", safariOptions);
                yield return new TestFixtureData(defaultBrowserVersion, defaultOS, chromeOptions);
                yield return new TestFixtureData(defaultBrowserVersion, defaultOS, edgeOptions);
                //one version back
                yield return new TestFixtureData("latest", "macOS 10.15", safariOptions);
                yield return new TestFixtureData("latest-1", defaultOS, chromeOptions);
                yield return new TestFixtureData("latest-1", defaultOS, edgeOptions);
                //duplication for more parallelization
                yield return new TestFixtureData("latest", "macOS 10.15", safariOptions);
                yield return new TestFixtureData(defaultBrowserVersion, defaultOS, chromeOptions);
                yield return new TestFixtureData(defaultBrowserVersion, defaultOS, edgeOptions);
                yield return new TestFixtureData("latest", "macOS 10.15", safariOptions);
                yield return new TestFixtureData(defaultBrowserVersion, defaultOS, chromeOptions);
                yield return new TestFixtureData(defaultBrowserVersion, defaultOS, edgeOptions);
                yield return new TestFixtureData("latest", "macOS 10.15", safariOptions);
                yield return new TestFixtureData(defaultBrowserVersion, defaultOS, chromeOptions);
                yield return new TestFixtureData(defaultBrowserVersion, defaultOS, edgeOptions);
                yield return new TestFixtureData("latest", "macOS 10.15", safariOptions);
                yield return new TestFixtureData(defaultBrowserVersion, defaultOS, chromeOptions);
                yield return new TestFixtureData(defaultBrowserVersion, defaultOS, edgeOptions);
                yield return new TestFixtureData("latest", "macOS 10.15", safariOptions);
                yield return new TestFixtureData(defaultBrowserVersion, defaultOS, chromeOptions);
                yield return new TestFixtureData(defaultBrowserVersion, defaultOS, edgeOptions);
                yield return new TestFixtureData("latest", "macOS 10.15", safariOptions);
                yield return new TestFixtureData(defaultBrowserVersion, defaultOS, chromeOptions);
                yield return new TestFixtureData(defaultBrowserVersion, defaultOS, edgeOptions);
                yield return new TestFixtureData("latest", "macOS 10.15", safariOptions);
                yield return new TestFixtureData(defaultBrowserVersion, defaultOS, chromeOptions);
                yield return new TestFixtureData(defaultBrowserVersion, defaultOS, edgeOptions);
                yield return new TestFixtureData("latest", "macOS 10.15", safariOptions);
                yield return new TestFixtureData(defaultBrowserVersion, defaultOS, chromeOptions);
                yield return new TestFixtureData(defaultBrowserVersion, defaultOS, edgeOptions);
                yield return new TestFixtureData("latest", "macOS 10.15", safariOptions);
                yield return new TestFixtureData(defaultBrowserVersion, defaultOS, chromeOptions);
                yield return new TestFixtureData(defaultBrowserVersion, defaultOS, edgeOptions);
                yield return new TestFixtureData("latest", "macOS 10.15", safariOptions);
                yield return new TestFixtureData(defaultBrowserVersion, defaultOS, chromeOptions);
                yield return new TestFixtureData(defaultBrowserVersion, defaultOS, edgeOptions);
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
                yield return new TestFixtureData("iPhone Simulator", "16.2");
            }
        }

        public static IEnumerable MostPopularAndroidDevices
        {
            get
            {
                yield return new TestFixtureData("Google Pixel.*", "Android", "Chrome");
                // duplication for massive parallel example
                yield return new TestFixtureData("Samsung.*", "Android", "Chrome");
            }
        }

        public static IEnumerable MostPopularIOSDevices
        {
            get
            {
                yield return new TestFixtureData("iPhone 14.*", "iOS", "Safari");
                yield return new TestFixtureData("iPhone 15.*", "iOS", "Safari");
                //duplication only for parallel example
                yield return new TestFixtureData("iPhone 14.*", "iOS", "Safari");
                yield return new TestFixtureData("iPhone 15.*", "iOS", "Safari");
            }
        }
    }
}