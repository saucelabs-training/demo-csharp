using OpenQA.Selenium;

namespace Selenium3.Nunit.Framework.BestPractices.Pages
{
    public class OrderConfirmationPage : BasePage
    {
        public OrderConfirmationPage(IWebDriver driver) : base(driver)
        {
        }

        public bool IsCheckoutComplete =>
            Wait.UntilIsVisibleByClass("complete-header").Text == "THANK YOU FOR YOUR ORDER";
    }
}