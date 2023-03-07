using Framework.Lib.Configuration.Models;
using Framework.Lib.Utils;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Framework.Lib.Configuration
{
    public class ConfigManager
    {
        public static IConfiguration Configuration =>
            new ConfigurationBuilder().AddJsonFile(PathHelper.GetAssemblyFile("appsettings.json")).Build();

        [JsonProperty(nameof(Options))]
        public static Options Options { get; set; }

        public static string? GetProperty(string section, string key) => Configuration.GetSection(section)[key];
        
        static ConfigManager()
        {
            var path = PathHelper.GetAssemblyFile("appsettings.json");
            JsonConvert.DeserializeObject<ConfigManager>(File.ReadAllText(path));
        }
    }
}
