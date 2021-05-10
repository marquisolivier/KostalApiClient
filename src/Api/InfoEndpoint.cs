using System.Threading.Tasks;
using KostalApiClient.Model;
using RestSharp;

namespace KostalApiClient.Api
{
    public class InfoEndpoint
    {      
        private readonly KostalRestClient _client;

        public InfoEndpoint(KostalRestClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Returns information about the API
        /// </summary>
        /// <returns></returns>
        public async Task<InfoVersion> GetVersion()
        {
            RestRequest request = new("/info/version") {RequestFormat = DataFormat.Json};
            return await _client.GetAsync<InfoVersion>(request);
        }
    }
}