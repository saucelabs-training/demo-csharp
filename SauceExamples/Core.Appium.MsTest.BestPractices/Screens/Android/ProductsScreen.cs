using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;

namespace Core.Appium.Nunit.BestPractices.Screens.Android
{
    public class ProductsScreen : BaseAndroidScreen
    {
        public ProductsScreen(AndroidDriver driver) : base(driver)
        {
        }

        public Action IsVisible()
        {
            return IsCartElementVisible;
        }

        private void IsCartElementVisible()
        {
            WaitFor.UntilIsVisible(By.XPath("//android.view.ViewGroup[@content-desc='test-Cart']"));
        }
    }
}