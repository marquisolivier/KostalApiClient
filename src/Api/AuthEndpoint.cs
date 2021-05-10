using System;
using System.Threading.Tasks;
using KostalApiClient.Model;
using RestSharp;

namespace KostalApiClient.Api
{
    public class AuthEndpoint
    {
        private readonly KostalRestClient _client;

        internal AuthEndpoint(KostalRestClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Returns information about the user
        /// </summary>
        /// <returns>Me</returns>
        public async Task<Me> GetMe()
        {
            RestRequest request = new ("/auth/me") {RequestFormat = DataFormat.Json};
            return await _client.GetAsync<Me>(request);
        }   
        
        /// <summary>
        /// Deletes the current session, the session id is no longer valid
        /// </summary>
        /// <returns></returns>
        public async Task Logout()
        {
            _client.CheckAuthentication();

            RestRequest request = new ("/auth/logout") {RequestFormat = DataFormat.Json};
            await _client.ExecutePostAsync(request);
            _client.IsAuthenticated = false;
        }

        /// <summary>
        /// Will not be implemented. Please use the standard way to set a password
        /// </summary>
        /// <returns></returns>
        public async Task SetPassword()
        {
            throw new NotImplementedException();
        }
    }
}