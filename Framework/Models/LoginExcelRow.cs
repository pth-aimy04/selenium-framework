namespace Lab9Automation.Models
{
    public class LoginExcelRow
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string ExpectedUrl { get; set; } = "";
        public string ExpectedError { get; set; } = "";
        public string Description { get; set; } = "";
    }
}