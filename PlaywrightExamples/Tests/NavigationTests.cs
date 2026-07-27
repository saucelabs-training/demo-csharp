using Microsoft.Playwright;
using NUnit.Framework;

namespace PlaywrightExamples.Tests;

[TestFixture]
[Parallelizable(ParallelScope.Fixtures)]
public class NavigationTests : TestBase
{
    [Test]
    public async Task NavigateToProductDetails()
    {
        await LoginAsync();

        await Page.Locator("[data-test='item-4-title-link']").ClickAsync();

        await Expect(Page).ToHaveURLAsync(new System.Text.RegularExpressions.Regex(@"inventory-item\.html\?id=4"));
        
        var productName = Page.Locator("[data-test='inventory-item-name']");
        await Expect(productName).ToHaveTextAsync("Sauce Labs Backpack");
    }

    [Test]
    public async Task NavigateBackToProducts()
    {
        await LoginAsync();

        await Page.Locator("[data-test='item-4-title-link']").ClickAsync();
        await Page.Locator("[data-test='back-to-products']").ClickAsync();

        await Expect(Page).ToHaveURLAsync("https://www.saucedemo.com/inventory.html");
    }

    [Test]
    public async Task NavigateToCart()
    {
        await LoginAsync();

        await Page.Locator(".shopping_cart_link").ClickAsync();

        await Expect(Page).ToHaveURLAsync("https://www.saucedemo.com/cart.html");
    }

    [Test]
    public async Task NavigateUsingBurgerMenu()
    {
        await LoginAsync();

        await Page.Locator("#react-burger-menu-btn").ClickAsync();
        await Page.Locator("#about_sidebar_link").ClickAsync();

        await Expect(Page).ToHaveURLAsync("https://saucelabs.com/");
    }

    [Test]
    public async Task ResetAppState()
    {
        await LoginAsync();

        // Add items to cart
        await Page.Locator("[data-test='add-to-cart-sauce-labs-backpack']").ClickAsync();
        await Page.Locator("[data-test='add-to-cart-sauce-labs-bolt-t-shirt']").ClickAsync();

        var cartBadge = Page.Locator(".shopping_cart_badge");
        await Expect(cartBadge).ToHaveTextAsync("2");

        // Reset app state
        await Page.Locator("#react-burger-menu-btn").ClickAsync();
        await Page.Locator("#reset_sidebar_link").ClickAsync();

        // Verify cart is empty
        await Expect(cartBadge).ToHaveCountAsync(0);
    }

    private static ILocatorAssertions Expect(ILocator locator) => Assertions.Expect(locator);
    private static IPageAssertions Expect(IPage page) => Assertions.Expect(page);
}
