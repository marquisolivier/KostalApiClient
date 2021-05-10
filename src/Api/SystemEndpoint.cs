using System.Threading.Tasks;
using RestSharp;

namespace KostalApiClient.Api
{
    public class SystemEndpoint
    {      
        private readonly KostalRestClient _client;

        public SystemEndpoint(KostalRestClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Returns information about the API
        /// </summary>
        /// <returns></returns>
        public async Task Reboot()
        {
            _client.CheckAuthentication();
            RestRequest request = new("/system/reboot") {RequestFormat = DataFormat.Json};
            await _client.ExecutePostAsync(request);
        }
    }
}