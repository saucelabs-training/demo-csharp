using Microsoft.Playwright;
using NUnit.Framework;

namespace PlaywrightExamples.Tests;

[TestFixture]
[Parallelizable(ParallelScope.Fixtures)]
public class SortingTests : TestBase
{
    [Test]
    public async Task SortByNameAtoZ()
    {
        await LoginAsync();

        await Page.Locator("[data-test='product-sort-container']").SelectOptionAsync("az");

        var firstProduct = Page.Locator("[data-test='inventory-item-name']").First;
        await Expect(firstProduct).ToHaveTextAsync("Sauce Labs Backpack");
    }

    [Test]
    public async Task SortByNameZtoA()
    {
        await LoginAsync();

        await Page.Locator("[data-test='product-sort-container']").SelectOptionAsync("za");

        var firstProduct = Page.Locator("[data-test='inventory-item-name']").First;
        await Expect(firstProduct).ToHaveTextAsync("Test.allTheThings() T-Shirt (Red)");
    }

    [Test]
    public async Task SortByPriceLowToHigh()
    {
        await LoginAsync();

        await Page.Locator("[data-test='product-sort-container']").SelectOptionAsync("lohi");

        var firstPrice = Page.Locator("[data-test='inventory-item-price']").First;
        await Expect(firstPrice).ToHaveTextAsync("$7.99");
    }

    [Test]
    public async Task SortByPriceHighToLow()
    {
        await LoginAsync();

        await Page.Locator("[data-test='product-sort-container']").SelectOptionAsync("hilo");

        var firstPrice = Page.Locator("[data-test='inventory-item-price']").First;
        await Expect(firstPrice).ToHaveTextAsync("$49.99");
    }

    private static ILocatorAssertions Expect(ILocator locator) => Assertions.Expect(locator);
}
