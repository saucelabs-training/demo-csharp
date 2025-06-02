using NUnit.Framework;
using OpenQA.Selenium;

namespace NUnitExamples.demo;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class CheckoutTests : TestBase
{
    [Test]
    public async Task BadInfo()
    {
        await RunWithReporting(async () =>
        {
            await StartChromeSessionAsync();
            
            Driver.Navigate().GoToUrl("https://www.saucedemo.com/");
            Driver.FindElement(By.CssSelector("input[data-test='username']")).SendKeys("standard_user");
            Driver.FindElement(By.CssSelector("input[data-test='password']")).SendKeys("secret_sauce");
            Driver.FindElement(By.CssSelector("input[data-test='login-button']")).Click();
            Driver.FindElement(By.CssSelector("button[data-test='add-to-cart-sauce-labs-onesie']")).Click();
            Driver.FindElement(By.ClassName("shopping_cart_link")).Click();
            Driver.FindElement(By.CssSelector("button[data-test='checkout']")).Click();
            Driver.FindElement(By.CssSelector("input[data-test='continue']")).Click();

            var classAttr = Driver.FindElement(By.CssSelector("input[data-test='firstName']")).GetAttribute("class");
            Assert.That(classAttr, Does.Contain("error"), "Expected error not found on page");
        });
    }

    [Test]
    public async Task GoodInfo()
    {
        await RunWithReporting(async () =>
        {
            await StartChromeSessionAsync();
            
            Driver.Navigate().GoToUrl("https://www.saucedemo.com/");
            Driver.FindElement(By.CssSelector("input[data-test='username']")).SendKeys("standard_user");
            Driver.FindElement(By.CssSelector("input[data-test='password']")).SendKeys("secret_sauce");
            Driver.FindElement(By.CssSelector("input[data-test='login-button']")).Click();
            Driver.FindElement(By.CssSelector("button[data-test='add-to-cart-sauce-labs-onesie']")).Click();
            Driver.FindElement(By.ClassName("shopping_cart_link")).Click();
            Driver.FindElement(By.CssSelector("button[data-test='checkout']")).Click();

            Driver.FindElement(By.CssSelector("input[data-test='firstName']")).SendKeys("Luke");
            Driver.FindElement(By.CssSelector("input[data-test='lastName']")).SendKeys("Perry");
            Driver.FindElement(By.CssSelector("input[data-test='postalCode']")).SendKeys("90210");
            Driver.FindElement(By.CssSelector("input[data-test='continue']")).Click();

            Assert.That(Driver.Url, Is.EqualTo("https://www.saucedemo.com/checkout-step-two.html"), 
                "Information Submission Unsuccessful");
        });
    }

    [Test]
    public async Task CompleteCheckout()
    {
        await RunWithReporting(async () =>
        {
            await StartChromeSessionAsync();
            
            Driver.Navigate().GoToUrl("https://www.saucedemo.com/");
            Driver.FindElement(By.CssSelector("input[data-test='username']")).SendKeys("standard_user");
            Driver.FindElement(By.CssSelector("input[data-test='password']")).SendKeys("secret_sauce");
            Driver.FindElement(By.CssSelector("input[data-test='login-button']")).Click();
            Driver.FindElement(By.CssSelector("button[data-test='add-to-cart-sauce-labs-onesie']")).Click();
            Driver.FindElement(By.ClassName("shopping_cart_link")).Click();
            Driver.FindElement(By.CssSelector("button[data-test='checkout']")).Click();

            Driver.FindElement(By.CssSelector("input[data-test='firstName']")).SendKeys("Luke");
            Driver.FindElement(By.CssSelector("input[data-test='lastName']")).SendKeys("Perry");
            Driver.FindElement(By.CssSelector("input[data-test='postalCode']")).SendKeys("90210");
            Driver.FindElement(By.CssSelector("input[data-test='continue']")).Click();
            Driver.FindElement(By.CssSelector("button[data-test='finish']")).Click();

            Assert.That(Driver.Url, Is.EqualTo("https://www.saucedemo.com/checkout-complete.html"));
            Assert.That(Driver.FindElement(By.ClassName("complete-text")).Displayed, Is.True);
        });
    }
}