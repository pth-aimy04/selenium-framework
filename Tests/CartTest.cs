using NUnit.Framework;
using Lab9Automation.Framework.Base;
using Lab9Automation.Framework.Pages;
using Lab9Automation.Framework.Config;
namespace Lab9Automation.Tests
{
    [TestFixture]
    public class CartTest : BaseTest
    {
        [Test]
        public void Add_First_Item_To_Cart_Success()
        {
            LoginPage loginPage = new LoginPage(Driver);

            CartPage cartPage = loginPage
                .Login(CredentialProvider.GetUsername(), CredentialProvider.GetPassword())
                .AddFirstItemToCart()
                .GoToCart();

            Assert.That(cartPage.GetItemCount(), Is.EqualTo(1));
        }

        [Test]
        public void Add_Item_By_Name_Success()
        {
            LoginPage loginPage = new LoginPage(Driver);

            CartPage cartPage = loginPage
                .Login(CredentialProvider.GetUsername(), CredentialProvider.GetPassword())
                .AddItemByName("Sauce Labs Backpack")
                .GoToCart();

            Assert.That(cartPage.GetItemCount(), Is.EqualTo(1));
            Assert.That(cartPage.GetItemNames(), Does.Contain("Sauce Labs Backpack"));
        }

        [Test]
        public void Remove_First_Item_From_Cart_Success()
        {
            LoginPage loginPage = new LoginPage(Driver);

            CartPage cartPage = loginPage
                .Login(CredentialProvider.GetUsername(), CredentialProvider.GetPassword())
                .AddFirstItemToCart()
                .GoToCart()
                .RemoveFirstItem();

            Assert.That(cartPage.GetItemCount(), Is.EqualTo(0));
        }
    }
}