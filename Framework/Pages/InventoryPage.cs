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

            var items = wait.Until(drv =>
            {
                var elements = drv.FindElements(inventoryItems);
                return elements.Count > 0 ? elements : null;
            });

            int beforeCount = GetCartItemCount();

            var firstButton = items[0].FindElement(By.TagName("button"));
            wait.Until(_ => firstButton.Displayed && firstButton.Enabled);
            firstButton.Click();

            WaitUntilCartCountChanges(beforeCount);
            return this;
        }

        public InventoryPage AddItemByName(string name)
        {
            WaitForPageLoad();

            var items = wait.Until(drv =>
            {
                var elements = drv.FindElements(inventoryItems);
                return elements.Count > 0 ? elements : null;
            });

            int beforeCount = GetCartItemCount();

            foreach (var item in items)
            {
                string itemName = item.FindElement(By.CssSelector(".inventory_item_name")).Text.Trim();

                if (itemName.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    var button = item.FindElement(By.TagName("button"));
                    wait.Until(_ => button.Displayed && button.Enabled);
                    button.Click();

                    WaitUntilCartCountChanges(beforeCount);
                    return this;
                }
            }

            throw new NoSuchElementException($"Không tìm thấy sản phẩm có tên: {name}");
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

        private void WaitUntilCartCountChanges(int oldCount)
        {
            wait.Until(_ => GetCartItemCount() > oldCount);
        }
    }
}