using Newtonsoft.Json;

namespace KostalApiClient.Model
{
    public class Setting
    {
        [JsonProperty("access")]
        public string Access { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("unit")]
        public string Unit { get; set; }
        [JsonProperty("max")]
        public string Max { get; set; }
        [JsonProperty("min")]
        public string Min { get; set; }
        [JsonProperty("default")]
        public string Default { get; set; }
    }
}