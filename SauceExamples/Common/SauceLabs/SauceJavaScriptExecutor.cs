using OpenQA.Selenium;

namespace Common.SauceLabs
{
    public class SauceJavaScriptExecutor
    {
        private IWebDriver _driver;

        public SauceJavaScriptExecutor(IWebDriver driver)
        {
            _driver = driver;
        }
        public void LogTestStatus(bool isPassed)
        {
            var script = "sauce:job-result=" + (isPassed ? "passed" : "failed");
            TryToExecuteScript(script);
        }

        private void TryToExecuteScript(string script)
        {
            try
            {
                ((IJavaScriptExecutor)_driver).ExecuteScript(script);
            }
            catch (System.Exception)
            {
                //This is a poor practice to catch a generic Exception
                //However, I temporarily did this for headless implementation
            }
        }

        public void LogMessage(string message)
        {
            TryToExecuteScript($"sauce:context={message}");
        }

        public void SetTestName(string testName)
        {
            TryToExecuteScript($"sauce:job-name={testName}");
        }

        public void SetBuildName(string buildName)
        {
            TryToExecuteScript($"sauce:job-build={buildName}");
        }
    }
}
