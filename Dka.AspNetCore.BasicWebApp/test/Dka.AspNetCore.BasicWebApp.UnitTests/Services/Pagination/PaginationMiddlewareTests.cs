using System.IO;
using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Services.Pagination;
using Dka.AspNetCore.BasicWebApp.ViewModels.Pagination;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Moq;
using Xunit;

namespace Dka.AspNetCore.BasicWebApp.UnitTests.Services.Pagination
{
    public class PaginationMiddlewareTests
    {
        [Fact]
        public async Task Test_PageSizeNotInQuery_CookieNotAdded_ShouldPass()
        {
            var defaultContext = new DefaultHttpContext();
            defaultContext.Response.Body = new MemoryStream();
            defaultContext.Request.Path = "/";

            var paginationMiddleware = new PaginationMiddleware(innerHttpContext => Task.CompletedTask);

            await paginationMiddleware.InvokeAsync(defaultContext);
            
            Assert.DoesNotContain(nameof(PaginationRequestViewModel.PageSize), defaultContext.Response.Headers[HeaderNames.SetCookie].ToString());
        }

        [Fact]
        public async Task Test_PageSizeInQuery_PageSizeNotValid_CookieNotAdded_ShouldPass()
        {
            var defaultContext = new DefaultHttpContext();
            defaultContext.Response.Body = new MemoryStream();
            defaultContext.Request.Path = "/";
            defaultContext.Request.QueryString.Add(nameof(PaginationRequestViewModel.PageSize), "adadasdasd");

            var paginationMiddleware = new PaginationMiddleware(innerHttpContext => Task.CompletedTask);

            await paginationMiddleware.InvokeAsync(defaultContext);
            
            Assert.DoesNotContain(nameof(PaginationRequestViewModel.PageSize), defaultContext.Response.Headers[HeaderNames.SetCookie].ToString());
        }

        [Fact]
        public async Task Test_PageSizeInQuery_PageSizeValid_CookieAdded_ShouldPass()
        {
            var defaultContext = new DefaultHttpContext();
            defaultContext.Response.Body = new MemoryStream();
            defaultContext.Request.Path = "/";
            defaultContext.Request.QueryString = defaultContext.Request.QueryString.Add(nameof(PaginationRequestViewModel.PageSize), "133");

            var paginationMiddleware = new PaginationMiddleware(innerHttpContext => Task.CompletedTask);
            
            await paginationMiddleware.InvokeAsync(defaultContext);
            
            Assert.Contains(nameof(PaginationRequestViewModel.PageSize), defaultContext.Response.Headers[HeaderNames.SetCookie].ToString());
        }
        
        [Fact]
        public void Test_UsePaginationMiddlewareMiddleware_ShouldPass()
        {
            var serviceCollection = new ServiceCollection();
            var provider = serviceCollection.BuildServiceProvider();
            var applicationBuilder = new ApplicationBuilder(provider);
            var newApplicationBuilder = applicationBuilder.UsePaginationMiddleware();
            Assert.NotNull(newApplicationBuilder);
        }        
    }
}