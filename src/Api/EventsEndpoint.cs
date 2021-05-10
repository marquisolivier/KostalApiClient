using System.Collections.Generic;
using System.Threading.Tasks;
using KostalApiClient.Model;
using RestSharp;

namespace KostalApiClient.Api
{
    public class EventsEndpoint
    {
        private readonly KostalRestClient _client;

        internal EventsEndpoint(KostalRestClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Get the latest events
        /// </summary>
        /// <returns>List of events</returns>
        public async Task<List<Event>> GetLastest()
        {
            _client.CheckAuthentication();
            RestRequest request = new("/events/latest") {RequestFormat = DataFormat.Json};
            return await _client.GetAsync<List<Event>>(request);
        }   

        /// <summary>
        /// Get the latest events with localized descriptions
        /// </summary>
        /// <returns>List of events</returns>
        public async Task<List<Event>> GetLastest(string language, int maxEvents)
        {
            _client.CheckAuthentication();
            RestRequest request = new("/events/latest") {RequestFormat = DataFormat.Json};
            request.AddJsonBody(new {language, max = maxEvents});
            IRestResponse<List<Event>> response = await _client.ExecutePostAsync<List<Event>>(request);
            return response.Data;
        }   
    }
}