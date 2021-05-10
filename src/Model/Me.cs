using System.Collections.Generic;
using Newtonsoft.Json;

namespace KostalApiClient.Model
{
    public class Me
    {
        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("locked")]
        public bool Locked { get; set; }

        [JsonProperty("anonymous")]
        public bool Anonymous { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("permissions")]
        public List<string> Permissions { get; set; }

        [JsonProperty("authenticated")]
        public bool Authenticated { get; set; }
    }
}