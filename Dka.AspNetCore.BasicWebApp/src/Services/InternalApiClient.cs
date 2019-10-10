using System.Net.Http;
using System.Threading.Tasks;

namespace Dka.AspNetCore.BasicWebApp.Services
{
    public class InternalApiClient : IInternalApiClient
    {
        private readonly HttpClient _httpClient;
        public InternalApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetPageNameAsync(string pageName)
        {
            var response = await _httpClient.GetAsync(
                $"/Pages/GetPageName?pagename={pageName}");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            return result;
        }
    }
}