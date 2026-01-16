using Common;
using OpenQA.Selenium.Appium.iOS;

namespace Core.Appium.Nunit.BestPractices.Screens.iOS
{
    public class BaseIosScreen
    {
        protected BaseIosScreen(IOSDriver driver)
        {
            Driver = driver;
            WaitFor = new Wait(Driver);
        }

        public Wait WaitFor { get; set; }

        public IOSDriver Driver { get; set; }
    }
}