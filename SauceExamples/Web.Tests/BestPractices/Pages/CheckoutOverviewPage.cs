using OpenQA.Selenium;
using Selenium3.Nunit.Framework.BestPractices.Elements;

namespace Selenium3.Nunit.Framework.BestPractices.Pages
{
    public class CheckoutOverviewPage : BasePage
    {
        public CheckoutOverviewPage(IWebDriver driver) : base(driver)
        {
        }
        public CartComponent Cart => new CartComponent(_driver);

        public OrderConfirmationPage FinishCheckout()
        {
            Wait.UntilIsVisibleByCss("[class='btn_action cart_button']").Click();
            return new OrderConfirmationPage(_driver);
        }

        internal CheckoutOverviewPage Open()
        {
            //TODO duplication here with the URL. Also happening in YourShoppingCartPage
            _driver.Navigate().GoToUrl($"{BaseUrl}/checkout-step-two.html");
            return this;
        }
    }
}