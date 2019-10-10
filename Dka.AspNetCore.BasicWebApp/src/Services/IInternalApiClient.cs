using System.Threading.Tasks;

namespace Dka.AspNetCore.BasicWebApp.Services
{
    public interface IInternalApiClient
    {
        Task<string> GetPageNameAsync(string pageName);
    }
}