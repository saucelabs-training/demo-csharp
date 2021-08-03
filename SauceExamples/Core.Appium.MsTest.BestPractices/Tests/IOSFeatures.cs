using Common.TestData;
using Core.Appium.Nunit.BestPractices.Screens.iOS;
using FluentAssertions;
using NUnit.Framework;

namespace Core.Appium.Nunit.BestPractices.Tests
{
    [TestFixtureSource(typeof(DeviceCombinations), nameof(DeviceCombinations.PopularIosDevices))]
    [Parallelizable]
    public class IosFeatures : IosTest
    {
        public IosFeatures(string deviceName) : base(deviceName)
        {
        }

        [Test]
        [Retry(1)]
        public void ShouldOpenApp()
        {
            var loginScreen = new LoginScreen(Driver);
            loginScreen.IsVisible().Should().NotThrow();
        }

        [Test]
        [Retry(1)]
        public void ShouldLogin()
        {
            var loginScreen = new LoginScreen(Driver);
            loginScreen.Login("standard_user", "secret_sauce");

            new ProductsScreen(Driver).IsVisible().Should().NotThrow();
        }
    }
}