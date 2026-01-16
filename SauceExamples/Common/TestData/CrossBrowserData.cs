using NUnit.Framework;
using System.Collections;

namespace Common.TestData
{
    public class CrossBrowserData
    {
        public static IEnumerable LatestConfigurations
        {
            get
            {
                //chrome on Mac
                yield return new TestFixtureData("Chrome", "latest", "macOS 10.13");
                yield return new TestFixtureData("Chrome", "latest-1", "macOS 10.13");
                yield return new TestFixtureData("Chrome", "latest-2", "macOS 10.13");

                //chrome on Windows(#1 platform as of 2019)
                yield return new TestFixtureData("Chrome", "latest", "Windows 10");
                yield return new TestFixtureData("Chrome", "latest-1", "Windows 10");
                yield return new TestFixtureData("Chrome", "latest-2", "Windows 10");

                //chrome on Windows 7(#3 platform as of 2019)
                yield return new TestFixtureData("Chrome", "latest", "Windows 7");
                yield return new TestFixtureData("Chrome", "latest-1", "Windows 7");
                yield return new TestFixtureData("Chrome", "latest-2", "Windows 7");

                //safari
                yield return new TestFixtureData("Safari", "latest", "macOS 10.13");
                //yield return new TestFixtureData("Safari", "latest-1", "macOS 10.12");
                //yield return new TestFixtureData("Safari", "10.0", "OS X 10.11");

                //firefox
                yield return new TestFixtureData("Firefox", "latest", "macOS 10.13");
                yield return new TestFixtureData("Firefox", "latest-1", "macOS 10.13");
                yield return new TestFixtureData("Firefox", "latest-2", "macOS 10.13");

                //edge
                yield return new TestFixtureData("MicrosoftEdge", "latest", "Windows 10");
                yield return new TestFixtureData("MicrosoftEdge", "latest-1", "Windows 10");
                yield return new TestFixtureData("MicrosoftEdge", "latest-2", "Windows 10");

                //IE
                yield return new TestFixtureData("Internet Explorer", "latest", "Windows 10");
                yield return new TestFixtureData("Internet Explorer", "latest", "Windows 7");
            }
        }

        public static IEnumerable LastTwoOnLinuxFirefoxChrome
        {
            get
            {
                yield return new TestFixtureData("Chrome", "latest", "Linux");
                yield return new TestFixtureData("Chrome", "latest-1", "Linux");
                yield return new TestFixtureData("Chrome", "latest-2", "Linux");

                yield return new TestFixtureData("Firefox", "latest", "Linux");
                yield return new TestFixtureData("Firefox", "latest-1", "Linux");
                yield return new TestFixtureData("Firefox", "latest-2", "Linux");
            }
        }

        public static IEnumerable LatestChrome
        {
            get
            {
                yield return new TestFixtureData("Chrome", "latest", "macOS 10.13");
            }
        }
    }
}