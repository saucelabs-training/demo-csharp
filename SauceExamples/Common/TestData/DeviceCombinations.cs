using System.Collections.Generic;

namespace Common.TestData
{
    public class DeviceCombinations
    {
        public static IEnumerable<object[]> PopularIosDevices => new[]
        {
            new object[] { "iPhone 11.*" },
            new object[] { "iPhone 12.*" },
            new object[] { "iPhone.*" },
        };

        public static IEnumerable<object[]> PopularAndroidDevices => new[]
        {
            new object[] { "Google Pixel.*", ""},
            new object[] { "Samsung Galaxy.*", ""},
        };
    }
}