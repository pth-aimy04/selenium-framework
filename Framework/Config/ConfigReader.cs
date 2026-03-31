using System.Text.Json;

namespace Lab9Automation.Framework.Config
{
    public class ConfigReader
    {
        private static ConfigModel? _config;

        public static string BaseUrl => _config?.BaseUrl ?? "";
        public static int ExplicitWait => _config?.ExplicitWait ?? 15;
        public static string ScreenshotPath => _config?.ScreenshotPath ?? "target/screenshots";

        public static void Load(string env = "dev")
        {
            string fileName = $"config-{env}.json";
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", fileName);

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Không tìm thấy file config: {filePath}");
            }

            string json = File.ReadAllText(filePath);
            _config = JsonSerializer.Deserialize<ConfigModel>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (_config == null)
            {
                throw new Exception("Đọc config thất bại.");
            }

            if (string.IsNullOrWhiteSpace(_config.BaseUrl))
            {
                throw new Exception("baseUrl trong file config đang rỗng hoặc sai key.");
            }
        }

        private class ConfigModel
        {
            public string BaseUrl { get; set; } = "";
            public int ExplicitWait { get; set; } = 15;
            public string ScreenshotPath { get; set; } = "target/screenshots";
        }
    }
}