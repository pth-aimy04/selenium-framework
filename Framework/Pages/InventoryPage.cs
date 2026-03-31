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
        private readonly By inventoryItems = By.CssSelector(".inventory_item");

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

            wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(inventoryItems));

            var addButtons = wait.Until(drv =>
            {
                var buttons = drv.FindElements(By.CssSelector("button[id^='add-to-cart-']"));
                return buttons.Count > 0 ? buttons : null;
            });

            addButtons[0].Click();

            wait.Until(drv =>
            {
                var removeButtons = drv.FindElements(By.CssSelector("button[id^='remove-']"));
                return removeButtons.Count > 0;
            });

            return this;
        }

        public InventoryPage AddItemByName(string name)
        {
            WaitForPageLoad();

            string slug = name.Trim().ToLower().Replace(" ", "-");
            By addButtonBy = By.Id($"add-to-cart-{slug}");
            By removeButtonBy = By.Id($"remove-{slug}");

            IWebElement addButton = wait.Until(ExpectedConditions.ElementToBeClickable(addButtonBy));
            addButton.Click();

            wait.Until(ExpectedConditions.ElementExists(removeButtonBy));

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
            WaitAndClick(cartLink);
            return new CartPage(driver);
        }
    }
}