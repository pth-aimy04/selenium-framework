using OpenQA.Selenium;
using Lab9Automation.Framework.Config;

namespace Lab9Automation.Framework.Utils
{
    public class ScreenshotUtil
    {
        public static string Capture(IWebDriver driver, string testName)
        {
            string rootPath = AppDomain.CurrentDomain.BaseDirectory;
            string folderPath = Path.Combine(rootPath, ConfigReader.ScreenshotPath);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string fileName = $"{testName}_{timestamp}.png";
            string fullPath = Path.Combine(folderPath, fileName);

            Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            screenshot.SaveAsFile(fullPath);

            return fullPath;
        }
    }
}