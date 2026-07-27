using Microsoft.Playwright;
using NUnit.Framework;

namespace PlaywrightExamples.Tests;

[TestFixture]
[Parallelizable(ParallelScope.Fixtures)]
public class CartTests : TestBase
{
    [Test]
    public async Task AddItemToCartFromInventory()
    {
        await LoginAsync();

        await Page.Locator("[data-test='add-to-cart-sauce-labs-backpack']").ClickAsync();

        var cartBadge = Page.Locator(".shopping_cart_badge");
        await Expect(cartBadge).ToHaveTextAsync("1");
    }

    [Test]
    public async Task AddMultipleItemsToCart()
    {
        await LoginAsync();

        await Page.Locator("[data-test='add-to-cart-sauce-labs-backpack']").ClickAsync();
        await Page.Locator("[data-test='add-to-cart-sauce-labs-bolt-t-shirt']").ClickAsync();
        await Page.Locator("[data-test='add-to-cart-sauce-labs-onesie']").ClickAsync();

        var cartBadge = Page.Locator(".shopping_cart_badge");
        await Expect(cartBadge).ToHaveTextAsync("3");
    }

    [Test]
    public async Task RemoveItemFromInventory()
    {
        await LoginAsync();

        await Page.Locator("[data-test='add-to-cart-sauce-labs-backpack']").ClickAsync();
        await Page.Locator("[data-test='remove-sauce-labs-backpack']").ClickAsync();

        var cartBadge = Page.Locator(".shopping_cart_badge");
        await Expect(cartBadge).ToHaveCountAsync(0);
    }

    [Test]
    public async Task RemoveItemFromCart()
    {
        await LoginAsync();

        await Page.Locator("[data-test='add-to-cart-sauce-labs-backpack']").ClickAsync();
        await Page.Locator(".shopping_cart_link").ClickAsync();
        await Page.Locator("[data-test='remove-sauce-labs-backpack']").ClickAsync();

        var cartBadge = Page.Locator(".shopping_cart_badge");
        await Expect(cartBadge).ToHaveCountAsync(0);
    }

    [Test]
    public async Task ContinueShoppingFromCart()
    {
        await LoginAsync();

        await Page.Locator("[data-test='add-to-cart-sauce-labs-backpack']").ClickAsync();
        await Page.Locator(".shopping_cart_link").ClickAsync();
        await Page.Locator("[data-test='continue-shopping']").ClickAsync();

        await Expect(Page).ToHaveURLAsync("https://www.saucedemo.com/inventory.html");
    }

    private static ILocatorAssertions Expect(ILocator locator) => Assertions.Expect(locator);
    private static IPageAssertions Expect(IPage page) => Assertions.Expect(page);
}
