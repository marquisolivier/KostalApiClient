using System.Collections.Generic;
using Newtonsoft.Json;

namespace KostalApiClient.Model
{
    public class InfoVersion
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("sw_version")]
        public string SoftwareVersion { get; set; }

        [JsonProperty("hostname")]
        public string Hostname { get; set; }

        [JsonProperty("api_version")]
        public string ApiVersion { get; set; }

    }

    public class ModuleSettings
    {
        [JsonProperty("settings")]
        public List<Setting> Settings { get; set; }
        [JsonProperty("moduleid")]
        public string ModuleId { get; set; }
    }


}