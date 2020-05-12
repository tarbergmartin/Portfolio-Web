using System.Net.Http;

namespace Portfolio.Components.Classes
{
    public class PortfolioApiClient
    {
        public HttpClient Client { get; private set; }

        public PortfolioApiClient(HttpClient client)
        {
            Client = client;
        }
    }
}
