using Newtonsoft.Json;

namespace Framework.Lib.Configuration.Models
{
    public class BrowserOptions
    {
        [JsonProperty(nameof(BrowserType))]
        public string BrowserType { get; set; }

        [JsonProperty(nameof(PageLoadTimeOutMs))]
        public int PageLoadTimeOutMs { get; set; }

        [JsonProperty(nameof(AsyncJsTimeoutMs))]
        public int AsyncJsTimeoutMs { get; set; }

        [JsonProperty(nameof(ImplicitWaitTimeOutMs))]
        public int ImplicitWaitTimeOutMs { get; set; }

        [JsonProperty(nameof(BookshopUrl))]
        public string BookshopUrl { get; set; }
    }
}