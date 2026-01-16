using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.iOS;

namespace Core.Appium.Nunit.BestPractices.Screens.iOS
{
    public class ProductsScreen : BaseIosScreen
    {
        public ProductsScreen(IOSDriver driver) : base(driver)
        {
        }

        public Action IsVisible()
        {
            return IsCartElementVisible;
        }

        private void IsCartElementVisible()
        {
            WaitFor.UntilIsVisible(By.Name("test-Cart"));
        }
    }
}