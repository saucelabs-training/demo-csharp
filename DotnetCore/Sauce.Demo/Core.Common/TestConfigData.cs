using System.Collections;
using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Safari;

namespace Core.Common
{
    public class TestConfigData
    {
        private const string DefaultBrowserVersion = "";
        private const string DefaultOs = "";

        private static readonly SafariOptions SafariOptions = new()
        {
            BrowserVersion = "latest",
            PlatformName = "macOS 10.15"
        };

        private static readonly ChromeOptions ChromeOptions = new()
        {
            BrowserVersion = "latest",
            PlatformName = "Windows 10",
            UseSpecCompliantProtocol = true
        };

        private static readonly EdgeOptions EdgeOptions = new()
        {
            BrowserVersion = "latest",
            PlatformName = "Windows 10"
        };

        public static IEnumerable PopularDesktopCombinations
        {
            get
            {
                yield return new TestFixtureData("latest", "macOS 10.15", SafariOptions);
                yield return new TestFixtureData(DefaultBrowserVersion, DefaultOs, ChromeOptions);
                yield return new TestFixtureData(DefaultBrowserVersion, DefaultOs, EdgeOptions);
                //one version back
                yield return new TestFixtureData("latest", "macOS 10.15", SafariOptions);
                yield return new TestFixtureData("latest-1", DefaultOs, ChromeOptions);
                yield return new TestFixtureData("latest-1", DefaultOs, EdgeOptions);
                //duplication for more parallelization
                yield return new TestFixtureData("latest", "macOS 10.15", SafariOptions);
                yield return new TestFixtureData(DefaultBrowserVersion, DefaultOs, ChromeOptions);
                yield return new TestFixtureData(DefaultBrowserVersion, DefaultOs, EdgeOptions);
                yield return new TestFixtureData("latest", "macOS 10.15", SafariOptions);
                yield return new TestFixtureData(DefaultBrowserVersion, DefaultOs, ChromeOptions);
                yield return new TestFixtureData(DefaultBrowserVersion, DefaultOs, EdgeOptions);
                yield return new TestFixtureData("latest", "macOS 10.15", SafariOptions);
                yield return new TestFixtureData(DefaultBrowserVersion, DefaultOs, ChromeOptions);
                yield return new TestFixtureData(DefaultBrowserVersion, DefaultOs, EdgeOptions);
                yield return new TestFixtureData("latest", "macOS 10.15", SafariOptions);
                yield return new TestFixtureData(DefaultBrowserVersion, DefaultOs, ChromeOptions);
                yield return new TestFixtureData(DefaultBrowserVersion, DefaultOs, EdgeOptions);
                yield return new TestFixtureData("latest", "macOS 10.15", SafariOptions);
                yield return new TestFixtureData(DefaultBrowserVersion, DefaultOs, ChromeOptions);
                yield return new TestFixtureData(DefaultBrowserVersion, DefaultOs, EdgeOptions);
                yield return new TestFixtureData("latest", "macOS 10.15", SafariOptions);
                yield return new TestFixtureData(DefaultBrowserVersion, DefaultOs, ChromeOptions);
                yield return new TestFixtureData(DefaultBrowserVersion, DefaultOs, EdgeOptions);
                yield return new TestFixtureData("latest", "macOS 10.15", SafariOptions);
                yield return new TestFixtureData(DefaultBrowserVersion, DefaultOs, ChromeOptions);
                yield return new TestFixtureData(DefaultBrowserVersion, DefaultOs, EdgeOptions);
                yield return new TestFixtureData("latest", "macOS 10.15", SafariOptions);
                yield return new TestFixtureData(DefaultBrowserVersion, DefaultOs, ChromeOptions);
                yield return new TestFixtureData(DefaultBrowserVersion, DefaultOs, EdgeOptions);
                yield return new TestFixtureData("latest", "macOS 10.15", SafariOptions);
                yield return new TestFixtureData(DefaultBrowserVersion, DefaultOs, ChromeOptions);
                yield return new TestFixtureData(DefaultBrowserVersion, DefaultOs, EdgeOptions);
                yield return new TestFixtureData("latest", "macOS 10.15", SafariOptions);
                yield return new TestFixtureData(DefaultBrowserVersion, DefaultOs, ChromeOptions);
                yield return new TestFixtureData(DefaultBrowserVersion, DefaultOs, EdgeOptions);
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

        public static IEnumerable AndroidSimulators
        {
            get
            {
                yield return new TestFixtureData("Google Pixel 3 XL GoogleAPI Emulator", "11.0");
                yield return new TestFixtureData("Google Pixel 3a GoogleAPI Emulator", "11.0");
            }
        }

        public static IEnumerable IOSSimulators
        {
            get
            {
                yield return new TestFixtureData("iPhone X Simulator", "14.0");
                yield return new TestFixtureData("iPhone XS Max Simulator", "14.0");
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
                yield return new TestFixtureData(SafariOptions, "375x812", "Iphone X");
                yield return new TestFixtureData(SafariOptions, "1280x1024", "1024p");
                yield return new TestFixtureData(SafariOptions, "1920x1080", "1080p");
                yield return new TestFixtureData(ChromeOptions, "412x732", "Pixel XL");
                yield return new TestFixtureData(ChromeOptions, "412x869", "Galaxy Note 10+");
            }
        }

        public static IEnumerable MostPopularAndroidDevices
        {
            get
            {
                yield return new TestFixtureData("Google Pixel.*", "Android", "Chrome");
                yield return new TestFixtureData("Google Pixel.*", "Android", "Chrome");
                yield return new TestFixtureData("Google Pixel.*", "Android", "Chrome");
                // duplication for massive parallel example
                yield return new TestFixtureData("HTC.*", "Android", "Chrome");
                yield return new TestFixtureData("HTC.*", "Android", "Chrome");
                yield return new TestFixtureData("Huawei.*", "Android", "Chrome");
            }
        }

        public static IEnumerable AndroidDevices
        {
            get
            {
                yield return new TestFixtureData("Google Pixel.*", "Android", "Chrome");
                yield return new TestFixtureData("HTC.*", "Android", "Chrome");
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