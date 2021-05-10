using System.Security.Authentication;
using RestSharp;

namespace KostalApiClient.Api
{
    public class KostalRestClient : RestClient
    {
        internal bool IsAuthenticated { get; set; }

        public KostalRestClient(string baseUrl) : base(baseUrl)
        {
        }

        public void CheckAuthentication()
        {
            if (!IsAuthenticated)
                throw new AuthenticationException("This endpoint requires authentication.");
        }
    }
}