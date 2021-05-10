using System.Collections.Generic;
using System.Threading.Tasks;
using KostalApiClient.Model;
using RestSharp;

namespace KostalApiClient.Api
{
    public class SettingsEndpoint
    {      
        private readonly KostalRestClient _client;

        internal SettingsEndpoint(KostalRestClient client)
        {
            _client = client;
        }

        public async Task<List<ModuleSettings>> GetSettings()
        {
            RestRequest request = new("/settings") {RequestFormat = DataFormat.Json};
            return await _client.GetAsync<List<ModuleSettings>>(request);
        }      
        
        public async Task<List<ModuleSettings>> GetSettings(string moduleId)
        {
            RestRequest request = new("/settings") {RequestFormat = DataFormat.Json};
            return await _client.GetAsync<List<ModuleSettings>>(request);
        }

        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 


        /*
            get /settings/{moduleid}

    Returns list of all available settings of a module
    get /settings/{moduleid}/{settingid}

    Returns specified setting of a module
    get /settings/{moduleid}/{settingids}

    Returns list of all modules with a list of available settings identifiers
    post /settings

    Returns a customized list of settings for one or more modules
    put /settings

    Writes a list of settings for one or more modules
         */
    }
}