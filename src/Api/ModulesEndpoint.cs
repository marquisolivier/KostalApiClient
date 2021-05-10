using System.Collections.Generic;
using System.Threading.Tasks;
using KostalApiClient.Model;
using RestSharp;

namespace KostalApiClient.Api
{
    public class ModulesEndpoint
    {
        private readonly KostalRestClient _client;

        internal ModulesEndpoint(KostalRestClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Returns list of all available modules (providing process data or settings)
        /// </summary>
        /// <returns></returns>
        public async Task<List<Module>> GetModules()
        {
            RestRequest request = new("/modules") {RequestFormat = DataFormat.Json};
            return await _client.GetAsync<List<Module>>(request);
        }
    }
}