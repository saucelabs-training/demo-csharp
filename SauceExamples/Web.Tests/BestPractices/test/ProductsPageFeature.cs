using FluentAssertions;
using NUnit.Framework;
using Selenium3.Nunit.Framework.BestPractices.Pages;

namespace Selenium3.Nunit.Framework.BestPractices.test
{
    [TestFixture]
    [TestFixtureSource(typeof(CrossBrowserData),
                nameof(CrossBrowserData.LatestConfigurations))]
    [Parallelizable]
    public class ProductsPageFeature : BaseTest
    {
        public ProductsPageFeature(string browser, string version, string os) :
            base(browser, version, os)
        {
        }
        [Test]
        public void ShouldHaveCorrectNumberOfProducts()
        {
            //IMPORTANT - how did I bypass the login here?
            var productsPage = new ProductsPage(Driver).Open();
            productsPage.ProductCount.Should().Be(6,
                    "we logged in successfully and we should have 6 items on the page");
        }
    }
}
