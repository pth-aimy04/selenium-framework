using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using Lab9Automation.Framework.Config;

namespace Lab9Automation.Framework.Base
{
    public abstract class BasePage
    {
        protected readonly IWebDriver driver;
        protected readonly WebDriverWait wait;

        protected BasePage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(ConfigReader.ExplicitWait));
        }

        /// <summary>
        /// Chờ phần tử có thể click rồi thực hiện click.
        /// </summary>
        protected void WaitAndClick(By locator)
        {
            IWebElement element = wait.Until(ExpectedConditions.ElementToBeClickable(locator));
            element.Click();
        }

        /// <summary>
        /// Chờ phần tử hiển thị, xóa dữ liệu cũ rồi nhập dữ liệu mới.
        /// </summary>
        protected void WaitAndType(By locator, string text)
        {
            IWebElement element = wait.Until(ExpectedConditions.ElementIsVisible(locator));
            element.Clear();
            element.SendKeys(text);
        }

        /// <summary>
        /// Lấy text của phần tử sau khi phần tử hiển thị.
        /// </summary>
        protected string GetText(By locator)
        {
            IWebElement element = wait.Until(ExpectedConditions.ElementIsVisible(locator));
            return element.Text.Trim();
        }

        /// <summary>
        /// Kiểm tra phần tử có hiển thị hay không, có xử lý stale element.
        /// </summary>
        protected bool IsElementVisible(By locator)
        {
            try
            {
                IWebElement element = wait.Until(ExpectedConditions.ElementExists(locator));
                return element.Displayed;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
            catch (StaleElementReferenceException)
            {
                try
                {
                    return driver.FindElement(locator).Displayed;
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Cuộn đến phần tử bằng JavaScript.
        /// </summary>
        protected void ScrollToElement(By locator)
        {
            IWebElement element = wait.Until(ExpectedConditions.ElementExists(locator));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", element);
        }

        /// <summary>
        /// Chờ cho trang load hoàn tất với document.readyState = complete.
        /// </summary>
        protected void WaitForPageLoad()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            wait.Until(_ => js.ExecuteScript("return document.readyState")?.ToString() == "complete");
        }

        /// <summary>
        /// Lấy giá trị attribute của phần tử.
        /// </summary>
        protected string GetAttribute(By locator, string attributeName)
        {
            IWebElement element = wait.Until(ExpectedConditions.ElementIsVisible(locator));
            return element.GetAttribute(attributeName)?.Trim() ?? string.Empty;
        }
    }
}