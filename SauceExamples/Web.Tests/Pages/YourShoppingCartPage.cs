﻿using OpenQA.Selenium;

namespace Web.Tests.Pages
{
    internal class YourShoppingCartPage : BasePage
    {
        public YourShoppingCartPage(IWebDriver driver) : base(driver)
        {
        }

        internal CheckoutInformationPage Checkout()
        {
            Wait.UntilIsVisible(By.ClassName("cart_checkout_link")).Click();
            return new CheckoutInformationPage(_driver);
        }
    }
}