using NUnit.Framework;
using Lab9Automation.Framework.Base;
using Lab9Automation.Framework.Pages;
using Lab9Automation.Framework.Utils;
using Lab9Automation.Models;

namespace Lab9Automation.Tests
{
    [TestFixture]
    public class LoginExcelTest : BaseTest
    {
        private static string ExcelPath =>
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "login_data.xlsx");

        public static IEnumerable<TestCaseData> SmokeCases()
        {
            var rows = ExcelReader.ReadLoginData(ExcelPath, "SmokeCases");

            foreach (var row in rows)
            {
                yield return new TestCaseData(row)
                    .SetName(row.Description)
                    .SetCategory("smoke");
            }
        }

        public static IEnumerable<TestCaseData> NegativeCases()
        {
            var rows = ExcelReader.ReadLoginData(ExcelPath, "NegativeCases");

            foreach (var row in rows)
            {
                yield return new TestCaseData(row)
                    .SetName(row.Description)
                    .SetCategory("regression");
            }
        }

        public static IEnumerable<TestCaseData> BoundaryCases()
        {
            var rows = ExcelReader.ReadLoginData(ExcelPath, "BoundaryCases");

            foreach (var row in rows)
            {
                yield return new TestCaseData(row)
                    .SetName(row.Description)
                    .SetCategory("regression");
            }
        }

        [TestCaseSource(nameof(SmokeCases))]
        public void Login_Smoke(LoginExcelRow data)
        {
            LoginPage login = new LoginPage(Driver);
            InventoryPage inventory = login.Login(data.Username, data.Password);

            Assert.That(inventory.IsLoaded(), Is.True);
            Assert.That(Driver.Url, Does.Contain(data.ExpectedUrl));
        }

        [TestCaseSource(nameof(NegativeCases))]
        public void Login_Negative(LoginExcelRow data)
        {
            LoginPage login = new LoginPage(Driver);
            login.LoginExpectingFailure(data.Username, data.Password);

            Assert.That(login.IsErrorDisplayed(), Is.True);
            Assert.That(login.GetErrorMessage(), Does.Contain(data.ExpectedError));
        }

        [TestCaseSource(nameof(BoundaryCases))]
        public void Login_Boundary(LoginExcelRow data)
        {
            LoginPage login = new LoginPage(Driver);
            login.LoginExpectingFailure(data.Username, data.Password);

            Assert.That(login.IsErrorDisplayed(), Is.True);
            Assert.That(login.GetErrorMessage(), Does.Contain(data.ExpectedError));
        }
    }
}