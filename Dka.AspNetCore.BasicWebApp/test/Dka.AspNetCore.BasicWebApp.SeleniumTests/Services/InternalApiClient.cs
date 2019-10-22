using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Services;

namespace Dka.AspNetCore.BasicWebApp.SeleniumTests.Services
{
    public class TestInternalApiClient : IInternalApiClient
    {
        public Task<string> GetPageNameAsync(string pageName)
        {
            return Task.FromResult(pageName);
        }
    }
}