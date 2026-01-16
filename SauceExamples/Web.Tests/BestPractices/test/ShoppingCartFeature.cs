using FluentAssertions;
using NUnit.Framework;
using Selenium3.Nunit.Framework.BestPractices.Pages;

namespace Selenium3.Nunit.Framework.BestPractices.test
{
    [TestFixture]
    [Parallelizable]
    [TestFixtureSource(typeof(CrossBrowserData),
                nameof(CrossBrowserData.LatestConfigurations))]
    public class ShoppingCartFeature : BaseTest
    {
        public ShoppingCartFeature(string browser, string browserVersion, string osPlatform) :
            base(browser, browserVersion, osPlatform)
        {
        }

        [Test]
        public void ShouldBeAbleToCheckOutWithItems()
        {
            //Arrange
            var overviewPage = new CheckoutOverviewPage(Driver);
            overviewPage.Open();
            //We don't need to actually use th UI to add items to the cart. 
            //I'm injecting Javascript to control the state of the cart
            //bypassing a bunch of unecessary element interactions and creating something efficient and stable
            overviewPage.Cart.InjectUserWithItems();
            //Act - very few UI interactions
            overviewPage.FinishCheckout().
                IsCheckoutComplete.Should().BeTrue("we finished the checkout process"); //Assert
        }
        [Test]
        public void ShouldBeAbleToAddItemToCart()
        {
            //Arrange
            var productsPage = new ProductsPage(Driver);
            //Act
            productsPage.Open();
            productsPage.AddFirstProductToCart();
            //Assert
            productsPage.Cart.ItemCount.Should().Be(1, "we added a backpack to the cart");
        }
    }
}
