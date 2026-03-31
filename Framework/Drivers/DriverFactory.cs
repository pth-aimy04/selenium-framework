using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;

namespace Lab9Automation.Framework.Drivers
{
    public class DriverFactory
    {
        public static IWebDriver CreateDriver(string browser)
        {
            bool isCI = Environment.GetEnvironmentVariable("CI") != null;

            switch (browser.ToLower())
            {
                case "edge":
                    return CreateEdgeDriver(isCI);

                case "chrome":
                default:
                    return CreateChromeDriver(isCI);
            }
        }

        private static IWebDriver CreateChromeDriver(bool headless)
        {
            ChromeOptions options = new ChromeOptions();

            if (headless)
            {
                options.AddArgument("--headless=new");
                options.AddArgument("--no-sandbox");
                options.AddArgument("--disable-dev-shm-usage");
                options.AddArgument("--window-size=1920,1080");
            }
            else
            {
                options.AddArgument("--start-maximized");
            }

            return new ChromeDriver(options);
        }

        private static IWebDriver CreateEdgeDriver(bool headless)
        {
            EdgeOptions options = new EdgeOptions();

            if (headless)
            {
                options.AddArgument("--headless=new");
                options.AddArgument("--no-sandbox");
                options.AddArgument("--disable-dev-shm-usage");
                options.AddArgument("--window-size=1920,1080");
            }
            else
            {
                options.AddArgument("start-maximized");
            }

            return new EdgeDriver(options);
        }
    }
}