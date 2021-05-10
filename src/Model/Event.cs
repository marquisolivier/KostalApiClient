using System;
using Newtonsoft.Json;

namespace KostalApiClient.Model
{
    public class Event
    {
        [JsonProperty("is_active")]
        public bool IsActive { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("category")]
        public string Category { get; set; }
        [JsonProperty("start_time")]
        public DateTime StartTime { get; set; }
        [JsonProperty("group")]
        public string Group { get; set; }
        [JsonProperty("code")]
        public int Code { get; set; }
        [JsonProperty("end_time")]
        public DateTime EndTime { get; set; }
        [JsonProperty("long_description")]
        public string LongDescription { get; set; }
    }
}