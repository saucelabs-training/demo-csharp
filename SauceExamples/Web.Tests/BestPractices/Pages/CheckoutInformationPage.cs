using OpenQA.Selenium;

namespace Selenium3.Nunit.Framework.BestPractices.Pages
{
    internal class CheckoutInformationPage : BasePage
    {
        public CheckoutInformationPage(IWebDriver driver) : base(driver)
        {
        }

        private By CartCheckoutButtonLocator => By.CssSelector("[class='btn_primary cart_button']");

        public CheckoutOverviewPage FillOutPersonalInformation()
        {
            Wait.UntilIsVisibleById("first-name").SendKeys("firstName");
            _driver.FindElement(By.Id("last-name")).SendKeys("lastName");
            _driver.FindElement(By.Id("postal-code")).SendKeys("zip");
            _driver.FindElement(CartCheckoutButtonLocator).Click();
            return new CheckoutOverviewPage(_driver);
        }
    }
}