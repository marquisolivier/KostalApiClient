using Newtonsoft.Json;

namespace KostalApiClient.Model
{
    public class Module
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}