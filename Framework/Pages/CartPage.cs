using OpenQA.Selenium;
using Lab9Automation.Framework.Base;

namespace Lab9Automation.Framework.Pages
{
    public class CartPage : BasePage
    {
        private readonly By cartItems = By.CssSelector(".cart_item");
        private readonly By removeButtons = By.CssSelector(".cart_button");
        private readonly By checkoutButton = By.Id("checkout");
        private readonly By cartList = By.CssSelector(".cart_list");

        public CartPage(IWebDriver driver) : base(driver)
        {
        }

        public int GetItemCount()
        {
            WaitForPageLoad();
            return driver.FindElements(cartItems).Count;
        }

        public CartPage RemoveFirstItem()
        {
            var buttons = driver.FindElements(removeButtons);
            if (buttons.Count > 0)
            {
                buttons[0].Click();
            }

            return this;
        }

        public CheckoutPage GoToCheckout()
        {
            WaitAndClick(checkoutButton);
            return new CheckoutPage(driver);
        }

        public List<string> GetItemNames()
        {
            List<string> names = new List<string>();

            var items = driver.FindElements(cartItems);
            foreach (var item in items)
            {
                string name = item.FindElement(By.CssSelector(".inventory_item_name")).Text.Trim();
                names.Add(name);
            }

            return names;
        }
    }
}