using Microsoft.Playwright;
using NUnit.Framework;

namespace PlaywrightExamples.Tests;

[TestFixture]
[Parallelizable(ParallelScope.Fixtures)]
public class CheckoutTests : TestBase
{
    [Test]
    public async Task CompleteCheckout()
    {
        await LoginAsync();

        // Add item to cart
        await Page.Locator("[data-test='add-to-cart-sauce-labs-backpack']").ClickAsync();
        await Page.Locator(".shopping_cart_link").ClickAsync();

        // Proceed to checkout
        await Page.Locator("[data-test='checkout']").ClickAsync();

        // Fill checkout information
        await Page.Locator("[data-test='firstName']").FillAsync("John");
        await Page.Locator("[data-test='lastName']").FillAsync("Doe");
        await Page.Locator("[data-test='postalCode']").FillAsync("12345");
        await Page.Locator("[data-test='continue']").ClickAsync();

        // Verify we're on overview page
        await Expect(Page).ToHaveURLAsync("https://www.saucedemo.com/checkout-step-two.html");

        // Complete checkout
        await Page.Locator("[data-test='finish']").ClickAsync();

        // Verify order completion
        var completeHeader = Page.Locator(".complete-header");
        await Expect(completeHeader).ToHaveTextAsync("Thank you for your order!");
    }

    [Test]
    public async Task CheckoutWithMissingFirstName()
    {
        await LoginAsync();

        await Page.Locator("[data-test='add-to-cart-sauce-labs-backpack']").ClickAsync();
        await Page.Locator(".shopping_cart_link").ClickAsync();
        await Page.Locator("[data-test='checkout']").ClickAsync();

        // Leave first name empty
        await Page.Locator("[data-test='lastName']").FillAsync("Doe");
        await Page.Locator("[data-test='postalCode']").FillAsync("12345");
        await Page.Locator("[data-test='continue']").ClickAsync();

        var errorElement = Page.Locator("[data-test='error']");
        await Expect(errorElement).ToContainTextAsync("Error: First Name is required");
    }

    [Test]
    public async Task CheckoutWithMissingLastName()
    {
        await LoginAsync();

        await Page.Locator("[data-test='add-to-cart-sauce-labs-backpack']").ClickAsync();
        await Page.Locator(".shopping_cart_link").ClickAsync();
        await Page.Locator("[data-test='checkout']").ClickAsync();

        await Page.Locator("[data-test='firstName']").FillAsync("John");
        // Leave last name empty
        await Page.Locator("[data-test='postalCode']").FillAsync("12345");
        await Page.Locator("[data-test='continue']").ClickAsync();

        var errorElement = Page.Locator("[data-test='error']");
        await Expect(errorElement).ToContainTextAsync("Error: Last Name is required");
    }

    [Test]
    public async Task CheckoutWithMissingPostalCode()
    {
        await LoginAsync();

        await Page.Locator("[data-test='add-to-cart-sauce-labs-backpack']").ClickAsync();
        await Page.Locator(".shopping_cart_link").ClickAsync();
        await Page.Locator("[data-test='checkout']").ClickAsync();

        await Page.Locator("[data-test='firstName']").FillAsync("John");
        await Page.Locator("[data-test='lastName']").FillAsync("Doe");
        // Leave postal code empty
        await Page.Locator("[data-test='continue']").ClickAsync();

        var errorElement = Page.Locator("[data-test='error']");
        await Expect(errorElement).ToContainTextAsync("Error: Postal Code is required");
    }

    [Test]
    public async Task CancelCheckout()
    {
        await LoginAsync();

        await Page.Locator("[data-test='add-to-cart-sauce-labs-backpack']").ClickAsync();
        await Page.Locator(".shopping_cart_link").ClickAsync();
        await Page.Locator("[data-test='checkout']").ClickAsync();

        await Page.Locator("[data-test='cancel']").ClickAsync();

        await Expect(Page).ToHaveURLAsync("https://www.saucedemo.com/cart.html");
    }

    private static ILocatorAssertions Expect(ILocator locator) => Assertions.Expect(locator);
    private static IPageAssertions Expect(IPage page) => Assertions.Expect(page);
}
