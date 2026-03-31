using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using Lab9Automation.Framework.Base;

namespace Lab9Automation.Framework.Pages
{
    public class InventoryPage : BasePage
    {
        private readonly By inventoryList = By.CssSelector(".inventory_list");
        private readonly By cartBadge = By.CssSelector(".shopping_cart_badge");
        private readonly By cartLink = By.CssSelector(".shopping_cart_link");

        public InventoryPage(IWebDriver driver) : base(driver)
        {
        }

        public bool IsLoaded()
        {
            WaitForPageLoad();
            return IsElementVisible(inventoryList);
        }

        public InventoryPage AddFirstItemToCart()
        {
            WaitForPageLoad();

            By firstAddButtonBy = By.CssSelector("button[id^='add-to-cart-']");
            IWebElement button = wait.Until(ExpectedConditions.ElementToBeClickable(firstAddButtonBy));

            SafeClick(button);

            wait.Until(drv =>
            {
                var badges = drv.FindElements(cartBadge);
                return badges.Count > 0 && badges[0].Text.Trim() == "1";
            });

            return this;
        }

        public InventoryPage AddItemByName(string name)
        {
            WaitForPageLoad();

            string slug = name.Trim().ToLower().Replace(" ", "-");
            By addButtonBy = By.Id($"add-to-cart-{slug}");

            IWebElement button = wait.Until(ExpectedConditions.ElementToBeClickable(addButtonBy));

            SafeClick(button);

            wait.Until(drv =>
            {
                var badges = drv.FindElements(cartBadge);
                return badges.Count > 0 && badges[0].Text.Trim() == "1";
            });

            return this;
        }

        public int GetCartItemCount()
        {
            try
            {
                var badges = driver.FindElements(cartBadge);
                if (badges.Count == 0)
                    return 0;

                string text = badges[0].Text.Trim();
                return int.TryParse(text, out int count) ? count : 0;
            }
            catch
            {
                return 0;
            }
        }

        public CartPage GoToCart()
        {
            wait.Until(ExpectedConditions.ElementToBeClickable(cartLink)).Click();
            return new CartPage(driver);
        }

        private void SafeClick(IWebElement element)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            try
            {
                js.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", element);
                wait.Until(_ => element.Displayed && element.Enabled);
                element.Click();
            }
            catch
            {
                js.ExecuteScript("arguments[0].click();", element);
            }
        }
    }
}