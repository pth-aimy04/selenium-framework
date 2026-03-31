using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Lab9Automation.Framework.Drivers
{
    public class DriverFactory
    {
        public static IWebDriver CreateDriver(string browser)
        {
            switch (browser.ToLower())
            {
                case "chrome":
                default:
                    ChromeOptions options = new ChromeOptions();
                    options.AddArgument("--start-maximized");
                    return new ChromeDriver(options);
            }
        }
    }
}