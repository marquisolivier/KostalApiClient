using System.Threading.Tasks;
using RestSharp;

namespace KostalApiClient.Api
{
    public class SystemEndpoint(KostalRestClient client)
    {
        /// <summary>
        /// Returns information about the API
        /// </summary>
        /// <returns></returns>
        public async Task Reboot()
        {
            client.CheckAuthentication();
            RestRequest request = new("/system/reboot") {RequestFormat = DataFormat.Json};
            await client.ExecutePostAsync(request);
        }
    }
}