namespace Lab9Automation.Framework.Config
{
    public static class CredentialProvider
    {
        public static string GetUsername()
        {
            string? username = Environment.GetEnvironmentVariable("SAUCEDEMO_USERNAME");

            if (!string.IsNullOrWhiteSpace(username))
                return username;

            return "standard_user";
        }

        public static string GetPassword()
        {
            string? password = Environment.GetEnvironmentVariable("SAUCEDEMO_PASSWORD");

            if (!string.IsNullOrWhiteSpace(password))
                return password;

            return "secret_sauce";
        }
    }
}