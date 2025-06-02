using NUnit.Framework;
using OpenQA.Selenium;

namespace NUnitExamples.demo;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class CartTest : TestBase
{
    [Test]
    public async Task AddFromProductPage()
    {
        await RunWithReporting(async () =>
        {
            await StartChromeSessionAsync();
            
            Driver.Navigate().GoToUrl("https://www.saucedemo.com/");
            Driver.FindElement(By.CssSelector("input[data-test='username']")).SendKeys("standard_user");
            Driver.FindElement(By.CssSelector("input[data-test='password']")).SendKeys("secret_sauce");
            Driver.FindElement(By.CssSelector("input[data-test='login-button']")).Click();

            Driver.FindElement(By.CssSelector("button[data-test='add-to-cart-sauce-labs-bolt-t-shirt']")).Click();

            Assert.That(Driver.FindElement(By.ClassName("shopping_cart_badge")).Text, Is.EqualTo("1"), 
                "Item not correctly added to cart");
        });
    }

    [Test]
    public async Task RemoveFromProductPage()
    {
        await RunWithReporting(async () =>
        {
            await StartChromeSessionAsync();
            
            Driver.Navigate().GoToUrl("https://www.saucedemo.com/");
            Driver.FindElement(By.CssSelector("input[data-test='username']")).SendKeys("standard_user");
            Driver.FindElement(By.CssSelector("input[data-test='password']")).SendKeys("secret_sauce");
            Driver.FindElement(By.CssSelector("input[data-test='login-button']")).Click();

            Driver.FindElement(By.CssSelector("button[data-test='add-to-cart-sauce-labs-bolt-t-shirt']")).Click();
            Driver.FindElement(By.CssSelector("button[data-test='remove-sauce-labs-bolt-t-shirt']")).Click();

            Assert.That(Driver.FindElements(By.ClassName("shopping_cart_badge")).Count, Is.EqualTo(0), 
                "Item not correctly removed from cart");
        });
    }

    [Test]
    public async Task AddFromInventoryPage()
    {
        await RunWithReporting(async () =>
        {
            await StartChromeSessionAsync();
            
            Driver.Navigate().GoToUrl("https://www.saucedemo.com/");
            Driver.FindElement(By.CssSelector("input[data-test='username']")).SendKeys("standard_user");
            Driver.FindElement(By.CssSelector("input[data-test='password']")).SendKeys("secret_sauce");
            Driver.FindElement(By.CssSelector("input[data-test='login-button']")).Click();

            Driver.FindElement(By.CssSelector("button[data-test='add-to-cart-sauce-labs-onesie']")).Click();

            Assert.That(Driver.FindElement(By.ClassName("shopping_cart_badge")).Text, Is.EqualTo("1"), 
                "Item not correctly added from inventory");
        });
    }

    [Test]
    public async Task RemoveFromInventoryPage()
    {
        await RunWithReporting(async () =>
        {
            await StartChromeSessionAsync();
            
            Driver.Navigate().GoToUrl("https://www.saucedemo.com/");
            Driver.FindElement(By.CssSelector("input[data-test='username']")).SendKeys("standard_user");
            Driver.FindElement(By.CssSelector("input[data-test='password']")).SendKeys("secret_sauce");
            Driver.FindElement(By.CssSelector("input[data-test='login-button']")).Click();

            Driver.FindElement(By.CssSelector("button[data-test='add-to-cart-sauce-labs-bike-light']")).Click();
            Driver.FindElement(By.CssSelector("button[data-test='remove-sauce-labs-bike-light']")).Click();

            Assert.That(Driver.FindElements(By.ClassName("shopping_cart_badge")).Count, Is.EqualTo(0), 
                "Shopping Cart is not empty");
        });
    }

    [Test]
    public async Task RemoveFromCartPage()
    {
        await RunWithReporting(async () =>
        {
            await StartChromeSessionAsync();
            
            Driver.Navigate().GoToUrl("https://www.saucedemo.com/");
            Driver.FindElement(By.CssSelector("input[data-test='username']")).SendKeys("standard_user");
            Driver.FindElement(By.CssSelector("input[data-test='password']")).SendKeys("secret_sauce");
            Driver.FindElement(By.CssSelector("input[data-test='login-button']")).Click();

            Driver.FindElement(By.CssSelector("button[data-test='add-to-cart-sauce-labs-backpack']")).Click();
            Driver.FindElement(By.ClassName("shopping_cart_link")).Click();
            Driver.FindElement(By.CssSelector("button[data-test='remove-sauce-labs-backpack']")).Click();

            Assert.That(Driver.FindElements(By.ClassName("shopping_cart_badge")).Count, Is.EqualTo(0), 
                "Shopping Cart is not empty");
        });
    }
}