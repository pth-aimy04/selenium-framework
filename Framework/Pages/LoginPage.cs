using OpenQA.Selenium;
using Lab9Automation.Framework.Base;

namespace Lab9Automation.Framework.Pages
{
    public class LoginPage : BasePage
    {
        private readonly By usernameField = By.Id("user-name");
        private readonly By passwordField = By.Id("password");
        private readonly By loginButton = By.Id("login-button");
        private readonly By errorMessage = By.CssSelector("[data-test='error']");

        public LoginPage(IWebDriver driver) : base(driver)
        {
        }

        public InventoryPage Login(string username, string password)
        {
            WaitAndType(usernameField, username);
            WaitAndType(passwordField, password);
            WaitAndClick(loginButton);
            return new InventoryPage(driver);
        }

        public LoginPage LoginExpectingFailure(string username, string password)
        {
            WaitAndType(usernameField, username);
            WaitAndType(passwordField, password);
            WaitAndClick(loginButton);
            return this;
        }

        public string GetErrorMessage()
        {
            return GetText(errorMessage);
        }

        public bool IsErrorDisplayed()
        {
            return IsElementVisible(errorMessage);
        }
    }
}