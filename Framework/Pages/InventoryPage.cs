using OpenQA.Selenium;
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
            var items = driver.FindElements(inventoryItems);
            if (items.Count > 0)
            {
                var firstButton = items[0].FindElement(By.TagName("button"));
                firstButton.Click();
            }
            return this;
        }

        public InventoryPage AddItemByName(string name)
        {
            var items = driver.FindElements(inventoryItems);

            foreach (var item in items)
            {
                string itemName = item.FindElement(By.CssSelector(".inventory_item_name")).Text.Trim();

                if (itemName.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    item.FindElement(By.TagName("button")).Click();
                    break;
                }
            }

            return this;
        }

        public int GetCartItemCount()
        {
            try
            {
                if (!IsElementVisible(cartBadge))
                    return 0;

                string text = GetText(cartBadge);
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