using FluentAssertions;
using NUnit.Framework;
using Selenium3.Nunit.Framework.BestPractices.Pages;
using Selenium3.Nunit.Scripts.BestPractices.CrossBrowserExamples;

namespace Selenium3.Nunit.Framework.Antipatterns
{
    [TestFixture]
    [TestFixtureSource(typeof(CrossBrowserData), "LatestConfigurations")]
    [NonParallelizable]
    [Ignore("Ignoring so that this can pass in CI")]
    public class PoorTests : BaseCrossBrowserTest
    {
        public PoorTests(string browser, string version, string os) :
            base(browser, version, os)
        {
        }

        [Test]
        public void EndToEndTest()
        {
            SauceReporter.SetBuildName("SCAntiPattern-NoSC");
            var loginPage = new SauceDemoLoginPage(Driver);
            //test loading of login page
            loginPage.Open().IsLoaded.Should().BeTrue("the login page should load successfully.");
            loginPage.UsernameField.Displayed.Should().BeTrue("the page is loaded, so the username field should exist");
            loginPage.PasswordField.Displayed.Should().BeTrue("the page is loaded, so the password field should exist");

            //test login with valid user
            var productsPage = loginPage.Login("standard_user", "secret_sauce");
            productsPage.IsLoaded.Should().BeTrue("we successfully logged in and the home page should load.");
            productsPage.Logout();
            loginPage.IsLoaded.Should().BeTrue("we successfully logged out, so the login page should be visible");

            //test login with Locked Out user
            productsPage = loginPage.Login("locked_out_user", "secret_sauce");
            productsPage.IsLoaded.Should().BeFalse("we used a locked out user who should not be able to login.");

            //test login with Problem user
            productsPage = loginPage.Login("problem_user", "secret_sauce");
            productsPage.IsLoaded.Should().BeTrue("we successfully logged in and the home page should load.");
            productsPage.Logout();

            //test login with invalid username
            productsPage = loginPage.Login("fake_user_name", "secret_sauce");
            productsPage.IsLoaded.Should().BeFalse("we used a invalid username who should not be able to login.");

            //test login with invalid password
            productsPage = loginPage.Login("standard_user", "fake_pass");
            productsPage.IsLoaded.Should().BeFalse("we used an invalid password, so the user should not be able to login");

            //validate that all products are present
            productsPage = loginPage.Login("standard_user", "secret_sauce");
            productsPage.IsLoaded.Should().BeTrue("we successfully logged in and the home page should load.");
            productsPage.ProductCount.Should().Be(6,
                "we logged in successfully and we should have 6 items on the page");

            //validate that a product can be added to a cart
            productsPage.AddFirstProductToCart();
            productsPage.Cart.ItemCount.Should().Be(1, "we added a backpack to the cart");

            //validate that user can checkout
            var cartPage = productsPage.Cart.Click();
            var checkoutOverviewPage = cartPage.Checkout().
                FillOutPersonalInformation();
            checkoutOverviewPage.FinishCheckout().IsCheckoutComplete.
                Should().
                BeTrue("we finished the checkout process");
        }
    }
}
