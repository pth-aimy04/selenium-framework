using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using Lab9Automation.Framework.Config;
using Lab9Automation.Framework.Drivers;
using Lab9Automation.Framework.Utils;

namespace Lab9Automation.Framework.Base
{
    public abstract class BaseTest
    {
        private static readonly ThreadLocal<IWebDriver?> tlDriver = new();

        protected IWebDriver Driver => tlDriver.Value!;

        [SetUp]
        public void SetUp()
        {
            string browser = TestContext.Parameters.Get("browser", "chrome");
            string env = TestContext.Parameters.Get("env", "dev");

            ConfigReader.Load(env);

            IWebDriver driver = DriverFactory.CreateDriver(browser);
            driver.Manage().Window.Maximize();

            tlDriver.Value = driver;

            Console.WriteLine($"[DEBUG] BaseUrl = {ConfigReader.BaseUrl}");
            Driver.Navigate().GoToUrl(ConfigReader.BaseUrl);
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed && tlDriver.Value != null)
                {
                    ScreenshotUtil.Capture(Driver, TestContext.CurrentContext.Test.Name);
                }
            }
            finally
            {
                if (tlDriver.Value != null)
                {
                    tlDriver.Value.Quit();
                    tlDriver.Value.Dispose();
                    tlDriver.Value = null;
                }
            }
        }
    }
}