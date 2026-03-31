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
            tlDriver.Value = driver;

            Driver.Manage().Window.Maximize();

            Console.WriteLine($"[DEBUG] Browser = {browser}");
            Console.WriteLine($"[DEBUG] Env = {env}");
            Console.WriteLine($"[DEBUG] BaseUrl = {ConfigReader.BaseUrl}");

            Driver.Navigate().GoToUrl(ConfigReader.BaseUrl);
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                if (tlDriver.Value != null &&
                    TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
                {
                    ScreenshotUtil.Capture(Driver, TestContext.CurrentContext.Test.Name);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[WARN] TearDown error: {ex.Message}");
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