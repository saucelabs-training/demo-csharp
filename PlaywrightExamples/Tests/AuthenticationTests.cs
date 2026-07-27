using Microsoft.Playwright;
using NUnit.Framework;

namespace PlaywrightExamples.Tests;

[TestFixture]
[Parallelizable(ParallelScope.Fixtures)]
public class AuthenticationTests : TestBase
{
    [Test]
    public async Task SignInSuccessful()
    {
        await Page.GotoAsync("https://www.saucedemo.com/");

        await Page.Locator("[data-test='username']").FillAsync("standard_user");
        await Page.Locator("[data-test='password']").FillAsync("secret_sauce");
        await Page.Locator("[data-test='login-button']").ClickAsync();

        await Expect(Page).ToHaveURLAsync("https://www.saucedemo.com/inventory.html");
    }

    [Test]
    public async Task SignInUnsuccessful_LockedOutUser()
    {
        await Page.GotoAsync("https://www.saucedemo.com/");

        await Page.Locator("[data-test='username']").FillAsync("locked_out_user");
        await Page.Locator("[data-test='password']").FillAsync("secret_sauce");
        await Page.Locator("[data-test='login-button']").ClickAsync();

        var errorElement = Page.Locator("[data-test='error']");
        await Expect(errorElement).ToContainTextAsync("Sorry, this user has been locked out");
    }

    [Test]
    public async Task SignInUnsuccessful_InvalidCredentials()
    {
        await Page.GotoAsync("https://www.saucedemo.com/");

        await Page.Locator("[data-test='username']").FillAsync("invalid_user");
        await Page.Locator("[data-test='password']").FillAsync("invalid_password");
        await Page.Locator("[data-test='login-button']").ClickAsync();

        var errorElement = Page.Locator("[data-test='error']");
        await Expect(errorElement).ToContainTextAsync("Username and password do not match");
    }

    [Test]
    public async Task Logout()
    {
        await LoginAsync();

        await Page.Locator("#react-burger-menu-btn").ClickAsync();
        await Page.Locator("#logout_sidebar_link").ClickAsync();

        await Expect(Page).ToHaveURLAsync("https://www.saucedemo.com/");
    }

    private static ILocatorAssertions Expect(ILocator locator) => Assertions.Expect(locator);
    private static IPageAssertions Expect(IPage page) => Assertions.Expect(page);
}
