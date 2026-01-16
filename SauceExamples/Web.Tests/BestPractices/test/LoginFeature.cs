using FluentAssertions;
using NUnit.Framework;
using Selenium3.Nunit.Framework.BestPractices.Pages;

namespace Selenium3.Nunit.Framework.BestPractices.test
{
    [TestFixture]
    [TestFixtureSource(typeof(CrossBrowserData),
        nameof(CrossBrowserData.LatestConfigurations))]
    [Parallelizable]
    public class LoginFeature : BaseTest
    {
        private SauceDemoLoginPage _loginPage;

        public LoginFeature(string browser, string version, string os) :
            base(browser, version, os)
        {
        }

        [Test]
        public void LoginPageShouldLoad()
        {
            _loginPage.Open().IsLoaded.Should().BeTrue("the login page should load successfully.");
        }
        [Test]
        public void ShouldBeAbleToLoginWithValidUser()
        {
            _loginPage.Open();
            var productsPage = _loginPage.Login("standard_user", "secret_sauce");
            productsPage.IsLoaded.Should().BeTrue("we successfully logged in and the home page should load.");
        }
        [Test]
        public void ShouldNotBeAbleToLoginWithLockedOutUser()
        {
            _loginPage.Open();
            var productsPage = _loginPage.Login("locked_out_user", "secret_sauce");
            productsPage.IsLoaded.Should().BeFalse("we used a locked out user who should not be able to login.");
        }
        [Test]
        public void ShouldBeAbleToLoginWithProblemUser()
        {
            _loginPage.Open();
            var productsPage = _loginPage.Login("problem_user", "secret_sauce");
            productsPage.IsLoaded.Should().BeTrue("we successfully logged in and the home page should load.");
        }
        [Test]
        public void ShouldNotLoginWithInvalidUserName()
        {
            _loginPage.Open();
            var productsPage = _loginPage.Login("FAKE_USER_NAME", "secret_sauce");
            productsPage.IsLoaded.Should().BeFalse("we used a invalid username who should not be able to login.");
        }
        [Test]
        public void ShouldNotLoginWithInvalidPassword()
        {
            _loginPage.Open();
            var productsPage = _loginPage.Login("problem_user", "FAKE_PASSWORD");
            productsPage.IsLoaded.Should().BeFalse("we used an invalid password, so the user should not be able to login");
        }

        [SetUp]
        public void RunBeforeEveryTest()
        {
            _loginPage = new SauceDemoLoginPage(Driver);
        }
    }
}
