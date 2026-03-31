using NUnit.Framework;
using Lab9Automation.Framework.Base;
using Lab9Automation.Framework.Pages;

namespace Lab9Automation.Tests
{
    [TestFixture]
    public class LoginTest : BaseTest
    {
        [Test]
        public void Login_Success()
        {
            LoginPage loginPage = new LoginPage(Driver);
            InventoryPage inventoryPage = loginPage.Login("standard_user", "secret_sauce");

            Assert.That(inventoryPage.IsLoaded(), Is.True, "Trang inventory chưa load thành công.");
        }

        [Test]
        public void Login_Fail_With_Wrong_Password()
        {
            LoginPage loginPage = new LoginPage(Driver);
            loginPage.LoginExpectingFailure("standard_user", "wrong_password");

            Assert.That(loginPage.IsErrorDisplayed(), Is.True, "Error message không hiển thị.");
        }
    }
}