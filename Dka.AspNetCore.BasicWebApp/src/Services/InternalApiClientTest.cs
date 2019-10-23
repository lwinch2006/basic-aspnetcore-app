using System.Net.Http;
using System.Threading.Tasks;

namespace Dka.AspNetCore.BasicWebApp.Services
{
    public class InternalApiClientTest : IInternalApiClient
    {
        public InternalApiClientTest(HttpClient httpClient)
        {}
        
        public Task<string> GetPageNameAsync(string pageName)
        {
            return Task.FromResult(pageName);
        }
    }
}