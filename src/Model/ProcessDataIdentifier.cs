using System.Collections.Generic;
using Newtonsoft.Json;

namespace KostalApiClient.Model
{
    public class ProcessDataIdentifier
    {
        [JsonProperty("processdataids")]
        public List<string> ProcessDataIds { get; set; }

        [JsonProperty("moduleid")]
        public string ModuleId { get; set; }
    }
}