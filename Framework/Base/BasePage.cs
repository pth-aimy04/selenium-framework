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
            wait.IgnoreExceptionTypes(
                typeof(NoSuchElementException),
                typeof(StaleElementReferenceException)
            );
        }

        protected void WaitAndClick(By locator)
        {
            IWebElement element = wait.Until(ExpectedConditions.ElementToBeClickable(locator));
            element.Click();
        }

        protected void WaitAndType(By locator, string text)
        {
            IWebElement element = wait.Until(ExpectedConditions.ElementIsVisible(locator));
            element.Clear();
            element.SendKeys(text);
        }

        protected string GetText(By locator)
        {
            IWebElement element = wait.Until(ExpectedConditions.ElementIsVisible(locator));
            return element.Text.Trim();
        }

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

        protected void ScrollToElement(By locator)
        {
            IWebElement element = wait.Until(ExpectedConditions.ElementExists(locator));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", element);
        }

        protected void WaitForPageLoad()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            wait.Until(_ => js.ExecuteScript("return document.readyState")?.ToString() == "complete");
        }

        protected string GetAttribute(By locator, string attributeName)
        {
            IWebElement element = wait.Until(ExpectedConditions.ElementIsVisible(locator));
            return element.GetAttribute(attributeName)?.Trim() ?? string.Empty;
        }
    }
}