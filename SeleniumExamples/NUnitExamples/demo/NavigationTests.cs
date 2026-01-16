using NUnit.Framework;
using OpenQA.Selenium;

namespace NUnitExamples.demo;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class NavigationTests : TestBase
{

    [Test]
    public void CancelFromCart()
    {
        Driver.Navigate().GoToUrl("https://www.saucedemo.com/");
        Driver.FindElement(By.CssSelector("input[data-test='username']")).SendKeys("standard_user");
        Driver.FindElement(By.CssSelector("input[data-test='password']")).SendKeys("secret_sauce");
        Driver.FindElement(By.CssSelector("input[data-test='login-button']")).Click();
        Driver.FindElement(By.ClassName("shopping_cart_link")).Click();
        Driver.FindElement(By.CssSelector("button[data-test='continue-shopping']")).Click();

        Assert.That(Driver.Url, Is.EqualTo("https://www.saucedemo.com/inventory.html"));
    }

    [Test]
    public void CancelFromInfoPage()
    {
        Driver.Navigate().GoToUrl("https://www.saucedemo.com/");
        Driver.FindElement(By.CssSelector("input[data-test='username']")).SendKeys("standard_user");
        Driver.FindElement(By.CssSelector("input[data-test='password']")).SendKeys("secret_sauce");
        Driver.FindElement(By.CssSelector("input[data-test='login-button']")).Click();
        Driver.FindElement(By.CssSelector("button[data-test='add-to-cart-sauce-labs-onesie']")).Click();
        Driver.FindElement(By.ClassName("shopping_cart_link")).Click();
        Driver.FindElement(By.CssSelector("button[data-test='checkout']")).Click();
        Driver.FindElement(By.CssSelector("button[data-test='cancel']")).Click();

        Assert.That(Driver.Url, Is.EqualTo("https://www.saucedemo.com/cart.html"));
    }

    [Test]
    public void CancelFromCheckoutPage()
    {
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
        Driver.FindElement(By.CssSelector("button[data-test='cancel']")).Click();

        Assert.That(Driver.Url, Is.EqualTo("https://www.saucedemo.com/inventory.html"));
    }

    [Test]
    public void StartCheckout()
    {
        Driver.Navigate().GoToUrl("https://www.saucedemo.com/");
        Driver.FindElement(By.CssSelector("input[data-test='username']")).SendKeys("standard_user");
        Driver.FindElement(By.CssSelector("input[data-test='password']")).SendKeys("secret_sauce");
        Driver.FindElement(By.CssSelector("input[data-test='login-button']")).Click();
        Driver.FindElement(By.CssSelector("button[data-test='add-to-cart-sauce-labs-onesie']")).Click();
        Driver.FindElement(By.ClassName("shopping_cart_link")).Click();
        Driver.FindElement(By.CssSelector("button[data-test='checkout']")).Click();

        Assert.That(Driver.Url, Is.EqualTo("https://www.saucedemo.com/checkout-step-one.html"));
    }
}