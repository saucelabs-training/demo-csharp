using OpenQA.Selenium;
using Selenium3.Nunit.Framework.BestPractices.Elements;

namespace Selenium3.Nunit.Framework.BestPractices.Pages
{
    public class ProductsPage : BasePage
    {
        private readonly string _pageUrlPart;

        public ProductsPage(IWebDriver driver) : base(driver)
        {
            _pageUrlPart = "inventory.html";
        }

        public bool IsLoaded => Wait.UntilIsDisplayedById("inventory_filter_container");

        private IWebElement LogoutLink => _driver.FindElement(By.Id("logout_sidebar_link"));

        private IWebElement HamburgerElement => _driver.FindElement(By.ClassName("bm-burger-button"));

        public int ProductCount =>
            _driver.FindElements(By.ClassName("inventory_item")).Count;

        public CartComponent Cart => new CartComponent(_driver);

        public void Logout()
        {
            HamburgerElement.Click();
            LogoutLink.Click();
        }

        internal ProductsPage Open()
        {
            _driver.Navigate().GoToUrl($"{BaseUrl}/{_pageUrlPart}");
            return this;
        }

        public void AddFirstProductToCart()
        {
            Wait.UntilIsVisibleByCss("button[class='btn_primary btn_inventory']").Click();
        }
    }
}