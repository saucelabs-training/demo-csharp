using OpenQA.Selenium;

namespace Selenium3.Nunit.Framework.BestPractices.Pages
{
    public class CheckoutCompletePage
    {
        private readonly IWebDriver _driver;
        public bool IsCheckedOut => _driver.Url.Contains("checkout-complete.html");


        public CheckoutCompletePage(IWebDriver driver)
        {
            _driver = driver;
        }
    }
}