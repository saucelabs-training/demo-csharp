using Common.TestData;
using Core.Appium.Nunit.BestPractices.Screens.Android;
using FluentAssertions;
using NUnit.Framework;

namespace Core.Appium.Nunit.BestPractices.Tests
{
    [TestFixtureSource(typeof(DeviceCombinations), nameof(DeviceCombinations.PopularAndroidDevices))]
    [Parallelizable]
    public class AndroidFeatures : AndroidTest
    {
        public AndroidFeatures(string deviceName, string deviceVersion) : base(deviceName, deviceVersion)
        {
        }

        [Test]
        public void ShouldOpenApp()
        {
            var loginScreen = new LoginScreen(Driver);
            loginScreen.IsVisible().Should().NotThrow();
        }

        [Test]
        public void ShouldLogin()
        {
            var loginScreen = new LoginScreen(Driver);
            loginScreen.Login("standard_user", "secret_sauce");

            new ProductsScreen(Driver).IsVisible().Should().NotThrow();
        }
    }
}