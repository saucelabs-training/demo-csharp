namespace Core.BestPractices.Web.Tests.Mobile
{
    public class MobileBaseTest : AllTestsBase
    {
        public readonly string DeviceName;
        public readonly string Platform;

        public MobileBaseTest(string deviceName, string platform)
        {
            DeviceName = deviceName;
            Platform = platform;
        }
    }
}