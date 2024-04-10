using System.Threading.Tasks;
using KostalApiClient.Model;
using RestSharp;

namespace KostalApiClient.Api
{
    public class InfoEndpoint(KostalRestClient client)
    {
        /// <summary>
        /// Returns information about the API
        /// </summary>
        /// <returns></returns>
        public async Task<InfoVersion> GetVersion()
        {
            RestRequest request = new("/info/version") {RequestFormat = DataFormat.Json};
            return await client.GetAsync<InfoVersion>(request);
        }
    }
}