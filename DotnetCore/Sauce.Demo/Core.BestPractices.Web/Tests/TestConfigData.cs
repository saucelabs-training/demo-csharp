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
            PlatformName = "Windows 10",
            UseSpecCompliantProtocol = true
        };

        private static readonly EdgeOptions edgeOptions = new()
        {
            BrowserVersion = "latest",
            PlatformName = "Windows 10"
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
                yield return new TestFixtureData("Google Pixel 3 XL GoogleAPI Emulator", "11.0");
                //duplication for more parallelization
                yield return new TestFixtureData("Google Pixel 3 XL GoogleAPI Emulator", "11.0");
                yield return new TestFixtureData("Google Pixel 3 XL GoogleAPI Emulator", "11.0");
                yield return new TestFixtureData("Google Pixel 3 XL GoogleAPI Emulator", "11.0");
                yield return new TestFixtureData("Google Pixel 3 XL GoogleAPI Emulator", "11.0");
                yield return new TestFixtureData("Google Pixel 3 XL GoogleAPI Emulator", "11.0");
                yield return new TestFixtureData("Google Pixel 3 XL GoogleAPI Emulator", "11.0");
            }
        }

        internal static IEnumerable PopularIOSSimulators
        {
            get
            {
                yield return new TestFixtureData("iPhone X Simulator", "14.0");
                yield return new TestFixtureData("iPhone X Simulator", "14.0");
                yield return new TestFixtureData("iPhone X Simulator", "14.0");
                yield return new TestFixtureData("iPhone X Simulator", "14.0");
                yield return new TestFixtureData("iPhone X Simulator", "14.0");
                yield return new TestFixtureData("iPhone X Simulator", "14.0");
                yield return new TestFixtureData("iPhone X Simulator", "14.0");
                yield return new TestFixtureData("iPhone X Simulator", "14.0");
                yield return new TestFixtureData("iPhone X Simulator", "14.0");
                yield return new TestFixtureData("iPhone X Simulator", "14.0");
            }
        }

        public static IEnumerable PopularVisualResolutions
        {
            get
            {
                yield return new TestFixtureData(safariOptions, "375x812", "Iphone X");
                yield return new TestFixtureData(safariOptions, "1280x1024", "1024p");
                yield return new TestFixtureData(safariOptions, "1920x1080", "1080p");
                yield return new TestFixtureData(chromeOptions, "412x732", "Pixel XL");
                yield return new TestFixtureData(chromeOptions, "412x869", "Galaxy Note 10+");
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
                yield return new TestFixtureData("iPhone 11.*", "iOS", "Safari");
                yield return new TestFixtureData("iPhone 12.*", "iOS", "Safari");
                //duplication only for parallel example
                yield return new TestFixtureData("iPhone 11.*", "iOS", "Safari");
                yield return new TestFixtureData("iPhone 12.*", "iOS", "Safari");
            }
        }
    }
}