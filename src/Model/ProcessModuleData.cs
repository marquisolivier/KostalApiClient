using System.Collections.Generic;
using Newtonsoft.Json;

namespace KostalApiClient.Model
{
    public class ProcessModuleData
    {
        [JsonProperty("processdata")]
        public List<ProcessData> ProcessDatas { get; set; }

        [JsonProperty("moduleid")]
        public string ModuleId { get; set; }
    }
}