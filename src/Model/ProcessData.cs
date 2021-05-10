using Newtonsoft.Json;

namespace KostalApiClient.Model
{
    public class ProcessData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("unit")]
        public string Unit { get; set; }

        [JsonProperty("value")]
        public double Value { get; set; }
    }
}