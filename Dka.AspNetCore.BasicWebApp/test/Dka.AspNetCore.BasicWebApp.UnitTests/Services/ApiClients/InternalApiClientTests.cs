using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Common.Models.Tenants;
using Dka.AspNetCore.BasicWebApp.Models.ApiClients;
using Dka.AspNetCore.BasicWebApp.Models.Tenants;
using Dka.AspNetCore.BasicWebApp.Services.ApiClients;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Moq.Protected;
using Xunit;

namespace Dka.AspNetCore.BasicWebApp.UnitTests.Services.ApiClients
{
    public class InternalApiClientTests
    {
        private InternalApiClient SetupInternalApiClient()
        {
            var httpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns<HttpRequestMessage, CancellationToken>((httpRequestMessage, cancellationToken) =>
                {
                    var response = new HttpResponseMessage();
                    response.StatusCode = HttpStatusCode.OK;
                    
                    if (httpRequestMessage.RequestUri.PathAndQuery.Equals("/Pages/GetPageName?pagename=home", StringComparison.OrdinalIgnoreCase))
                    {
                        response.StatusCode = HttpStatusCode.OK;
                        response.Content = new StringContent("home");
                    }
                    
                    if (httpRequestMessage.RequestUri.PathAndQuery.Equals("/Pages/GetPageName?pagename=about", StringComparison.OrdinalIgnoreCase))
                    {
                        response.StatusCode = HttpStatusCode.OK;
                        response.Content = new StringContent("about");
                    }
                    
                    if (httpRequestMessage.RequestUri.PathAndQuery.Equals("/Administration/Tenants", StringComparison.OrdinalIgnoreCase))
                    {
                        response.StatusCode = HttpStatusCode.OK;
                        response.Content = new StringContent(JsonSerializer.Serialize(Tenant.GetDummyTenantSet().GetAwaiter().GetResult()));
                        response.Content.Headers.ContentType.MediaType = "application/json";
                    }
                    
                    if (httpRequestMessage.RequestUri.PathAndQuery.Equals("/Administration/Tenants/Details/9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C", StringComparison.OrdinalIgnoreCase))
                    {
                        var tenants = Tenant.GetDummyTenantSet().GetAwaiter().GetResult().ToList();
                        response.StatusCode = HttpStatusCode.OK;

                        if (tenants.Find(record => record.Guid == new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C")) is { } foundTenant)
                        {
                            response.Content = new StringContent(JsonSerializer.Serialize(foundTenant));
                            response.Content.Headers.ContentType.MediaType = "application/json";
                        }
                        else
                        {
                            response.StatusCode = HttpStatusCode.NotFound;
                        }
                    }
                    
                    if (httpRequestMessage.RequestUri.PathAndQuery.Equals("/Administration/Tenants/Details/9D5CC1D7-EA23-43AB-8725-01D8EBF0BFFF", StringComparison.OrdinalIgnoreCase))
                    {
                        var tenants = Tenant.GetDummyTenantSet().GetAwaiter().GetResult().ToList();
                        response.StatusCode = HttpStatusCode.OK;

                        if (tenants.Find(record => record.Guid == new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0BFFF")) is { } foundTenant)
                        {
                            response.Content = new StringContent(JsonSerializer.Serialize(foundTenant));
                            response.Content.Headers.ContentType.MediaType = "application/json";
                        }
                        else
                        {
                            response.StatusCode = HttpStatusCode.NotFound;
                        }
                    }
                    
                    if (httpRequestMessage.RequestUri.PathAndQuery.Equals("/Administration/Tenants/delete/9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C", StringComparison.OrdinalIgnoreCase))
                    {
                        var tenants = Tenant.GetDummyTenantSet().GetAwaiter().GetResult().ToList();
                        response.StatusCode = HttpStatusCode.OK;

                        if (tenants.Find(record => record.Guid == new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C")) is { } foundTenant)
                        {
                            tenants.Remove(foundTenant);
                            response.Content = new StringContent(JsonSerializer.Serialize(foundTenant));
                            response.Content.Headers.ContentType.MediaType = "application/json";
                        }
                        else
                        {
                            response.StatusCode = HttpStatusCode.NotFound;
                        }
                    }
                    
                    if (httpRequestMessage.RequestUri.PathAndQuery.Equals("/Administration/Tenants/delete/9D5CC1D7-EA23-43AB-8725-01D8EBF0BFFF", StringComparison.OrdinalIgnoreCase))
                    {
                        var tenants = Tenant.GetDummyTenantSet().GetAwaiter().GetResult().ToList();
                        response.StatusCode = HttpStatusCode.OK;

                        if (tenants.Find(record => record.Guid == new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0BFFF")) is { } foundTenant)
                        {
                            tenants.Remove(foundTenant);
                            response.Content = new StringContent(JsonSerializer.Serialize(foundTenant));
                            response.Content.Headers.ContentType.MediaType = "application/json";
                        }
                        else
                        {
                            response.StatusCode = HttpStatusCode.NotFound;
                        }
                    }                     
                    
                    if (httpRequestMessage.RequestUri.PathAndQuery.Equals("/Administration/Tenants/edit/9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C", StringComparison.OrdinalIgnoreCase))
                    {
                        var tenants = Tenant.GetDummyTenantSet().GetAwaiter().GetResult().ToList();
                        response.StatusCode = HttpStatusCode.OK;

                        if (tenants.Find(record => record.Guid == new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C")) is { } foundTenant)
                        {
                            var tenantContract =
                                httpRequestMessage.Content.ReadAsAsync<Common.Models.ApiContracts.Tenant>(cancellationToken).GetAwaiter().GetResult();

                            foundTenant.Name = tenantContract.Name;
                            foundTenant.Alias = tenantContract.Alias;
                            
                            response.StatusCode = HttpStatusCode.NoContent;
                        }
                        else
                        {
                            response.StatusCode = HttpStatusCode.NotFound;
                        }
                    } 
                    
                    if (httpRequestMessage.RequestUri.PathAndQuery.Equals("/Administration/Tenants/edit/9D5CC1D7-EA23-43AB-8725-01D8EBF0BFFF", StringComparison.OrdinalIgnoreCase))
                    {
                        var tenants = Tenant.GetDummyTenantSet().GetAwaiter().GetResult().ToList();
                        response.StatusCode = HttpStatusCode.OK;

                        if (tenants.Find(record => record.Guid == new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0BFFF")) is { } foundTenant)
                        {
                            var tenantContract =
                                httpRequestMessage.Content.ReadAsAsync<Common.Models.ApiContracts.Tenant>(cancellationToken).GetAwaiter().GetResult();

                            foundTenant.Name = tenantContract.Name;
                            foundTenant.Alias = tenantContract.Alias;
                            
                            response.StatusCode = HttpStatusCode.NoContent;
                        }
                        else
                        {
                            response.StatusCode = HttpStatusCode.NotFound;
                        }
                    }                     
                    
                    if (httpRequestMessage.RequestUri.PathAndQuery.Equals("/Administration/Tenants/new", StringComparison.OrdinalIgnoreCase))
                    {
                        var tenants = Tenant.GetDummyTenantSet().GetAwaiter().GetResult().ToList();
                        response.StatusCode = HttpStatusCode.OK;

                        var newTenantContract =
                            httpRequestMessage.Content.ReadAsAsync<Common.Models.ApiContracts.NewTenant>(cancellationToken).GetAwaiter().GetResult();
                        
                        var newTenantBo = new Tenant
                        {
                            Name = newTenantContract.Name,
                            Alias = newTenantContract.Alias,
                            Id = tenants.Max(record => record.Id) + 1,
                            Guid = new Guid("C78C30E4-620E-4E2C-8BAF-2A81BA8470A1")
                        };
                        
                        tenants.Add(newTenantBo);
                        
                        response.Content = new StringContent(JsonSerializer.Serialize(tenants[^1].Guid));
                        response.Content.Headers.ContentType.MediaType = "application/json";
                    } 
                    
                    if (httpRequestMessage.RequestUri.PathAndQuery.Equals("/health/ready", StringComparison.OrdinalIgnoreCase))
                    {
                        response.StatusCode = HttpStatusCode.OK;
                        response.Content = new StringContent(HealthStatus.Healthy.ToString());
                    }
                    
                    if (httpRequestMessage.RequestUri.PathAndQuery.Equals("/health/live", StringComparison.OrdinalIgnoreCase))
                    {
                        response.StatusCode = HttpStatusCode.OK;
                        response.Content = new StringContent(HealthStatus.Healthy.ToString());
                    }
                    
                    if (httpRequestMessage.RequestUri.PathAndQuery.Equals("/health", StringComparison.OrdinalIgnoreCase))
                    {
                        response.StatusCode = HttpStatusCode.OK;
                        response.Content = new StringContent(HealthStatus.Healthy.ToString());
                    }                     
                    

                    return Task.FromResult(response);
                });

            var httpClient = new HttpClient(httpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://localhost")
            };
            
            var httpContextAccessor = new HttpContextAccessor();
            
            var internalApiClient = new InternalApiClient(httpClient, httpContextAccessor);
            return internalApiClient;
        }
        
        private InternalApiClient SetupInternalApiClientWithInternalServerErrorException()
        {
            var httpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns<HttpRequestMessage, CancellationToken>((httpRequestMessage, cancellationToken) =>
                {
                    var response = new HttpResponseMessage();
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    return Task.FromResult(response);
                });

            var httpClient = new HttpClient(httpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://localhost")
            };
            
            var httpContextAccessor = new HttpContextAccessor();
            var internalApiClient = new InternalApiClient(httpClient, httpContextAccessor);
            return internalApiClient;
        }
        
        private InternalApiClient SetupInternalApiClientWithNullResponse()
        {
            var httpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns<HttpRequestMessage, CancellationToken>((httpRequestMessage, cancellationToken) => Task.FromResult((HttpResponseMessage) null));

            var httpClient = new HttpClient(httpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://localhost")
            };
            
            var httpContextAccessor = new HttpContextAccessor();
            var internalApiClient = new InternalApiClient(httpClient, httpContextAccessor);
            return internalApiClient;
        }
        
        private InternalApiClient SetupInternalApiClientWithGeneralException()
        {
            var httpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns<HttpRequestMessage, CancellationToken>((httpRequestMessage, cancellationToken) => throw new Exception());

            var httpClient = new HttpClient(httpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://localhost")
            };
            
            var httpContextAccessor = new HttpContextAccessor();
            var internalApiClient = new InternalApiClient(httpClient, httpContextAccessor);
            return internalApiClient;
        }
        
        private InternalApiClient SetupInternalApiClientWithHttpRequestException()
        {
            var httpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns<HttpRequestMessage, CancellationToken>((httpRequestMessage, cancellationToken) => throw new HttpRequestException());

            var httpClient = new HttpClient(httpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://localhost")
            };
            
            var httpContextAccessor = new HttpContextAccessor();
            var internalApiClient = new InternalApiClient(httpClient, httpContextAccessor);
            return internalApiClient;
        }         

        [Fact]
        public void TestingCreation_ShouldPass()
        {
            var internalApiClient = SetupInternalApiClient();
            Assert.NotNull(internalApiClient);
        }

        [Theory]
        [InlineData("Home")]
        [InlineData("About")]
        public async Task TestingGetPageNameAsync_ShouldPass(string pageName)
        {
            var internalApiClient = SetupInternalApiClient();

            var result = await internalApiClient.GetPageNameAsync(pageName);

            Assert.Equal(pageName.ToLower(), result.ToLower());
        }

        [Fact]
        public async Task TestingGetPageNameAsync_ThrowingException__ShouldPass()
        {
            try
            {
                var internalApiClient = SetupInternalApiClientWithInternalServerErrorException();

                await internalApiClient.GetPageNameAsync("Status");
            }
            catch (ApiConnectionException)
            {
                Assert.True(true, "ApiConnectionException is thrown. This behaviour is legal.");
            }
        }

        [Fact]
        public async Task TestingGetTenants_ShouldPass()
        {
            var internalApiClient = SetupInternalApiClient();

            var result = (await internalApiClient.GetTenants()).ToList();

            Assert.Equal(3, result.Count);
        }

        [Fact]
        public async Task TestingGetTenants_ThrowingException_ShouldPass()
        {
            try
            {
                var internalApiClient = SetupInternalApiClientWithInternalServerErrorException();

                await internalApiClient.GetTenants();
            }
            catch (ApiConnectionException)
            {
                Assert.True(true, "ApiConnectionException is thrown. This behaviour is legal.");
                return;
            }
            
            Assert.False(true, "ApiConnectionException is not thrown. This behaviour is illegal.");
        }

        [Fact]
        public async Task TestingGetTenantByGuid_PassingValidGuid_ShouldPass()
        {
            var internalApiClient = SetupInternalApiClient();

            var result = await internalApiClient.GetTenantByGuid(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C"));

            Assert.NotNull(result);
            Assert.Equal("Umbrella Corporation", result.Name);
        }

        [Fact]
        public async Task TestingGetTenantByGuid_PassingInvalidGuid_ThrowingTenantNotFoundException_ShouldPass()
        {
            try
            {
                var internalApiClient = SetupInternalApiClient();

                await internalApiClient.GetTenantByGuid(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0BFFF"));
            }
            catch (TenantNotFoundException)
            {
                Assert.True(true, "TenantNotFoundException is thrown. This behaviour is legal.");
                return;
            }
            
            Assert.True(false, "TenantNotFoundException is not thrown. This behaviour is illegal.");
        }

        [Fact]
        public async Task TestingGetTenantByGuid_ThrowingApiStatusCodeException_ShouldPass()
        {
            try
            {
                var internalApiClient = SetupInternalApiClientWithInternalServerErrorException();

                await internalApiClient.GetTenantByGuid(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C"));
            }
            catch (ApiStatusCodeException)
            {
                Assert.True(true, "ApiStatusCodeException is thrown. This behaviour is legal.");
                return;
            }          
            
            Assert.True(false, "ApiStatusCodeException is not thrown. This behaviour is illegal.");
        }
        
        [Fact]
        public async Task TestingGetTenantByGuid_NullResponse_ThrowingApiConnectionException_ShouldPass()
        {
            try
            {
                var internalApiClient = SetupInternalApiClientWithNullResponse();

                await internalApiClient.GetTenantByGuid(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C"));
            }
            catch (ApiConnectionException)
            {
                Assert.True(true, "ApiConnectionException is thrown. This behaviour is legal.");
                return;
            }
            
            Assert.True(false, "ApiConnectionException is not thrown. This behaviour is illegal.");
        }
        
        [Fact]
        public async Task TestingGetTenantByGuid_GeneralException_ThrowingApiConnectionException_ShouldPass()
        {
            try
            {
                var internalApiClient = SetupInternalApiClientWithGeneralException();

                await internalApiClient.GetTenantByGuid(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C"));
            }
            catch (ApiConnectionException)
            {
                Assert.True(true, "ApiConnectionException is thrown. This behaviour is legal.");
                return;
            }
            
            Assert.True(false, "ApiConnectionException is not thrown. This behaviour is illegal.");
        }

        [Fact]
        public async Task TestingGetTenantByGuid_HttpRequestException_ThrowingApiConnectionException_ShouldPass()
        {
            try
            {
                var internalApiClient = SetupInternalApiClientWithHttpRequestException();

                await internalApiClient.GetTenantByGuid(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C"));
            }
            catch (ApiConnectionException)
            {
                Assert.True(true, "ApiConnectionException is thrown. This behaviour is legal.");
                return;
            }
            
            Assert.True(false, "ApiConnectionException is not thrown. This behaviour is illegal.");
        }

        [Fact]
        public async Task TestingCreateNewTenant_PassingValidTenant_ShouldPass()
        {
            var internalApiClient = SetupInternalApiClient();

            var result = await internalApiClient.CreateNewTenant(new Common.Models.ApiContracts.NewTenant
            {
                Name = "Test company 1",
                Alias = "test-company-1"
            });
            
            Assert.Equal(new Guid("C78C30E4-620E-4E2C-8BAF-2A81BA8470A1"), result);
        }

        [Fact]
        public async Task TestingCreateNewTenant_PassingNullTenant_ThrowingApiConnectionException_ShouldPass()
        {
            try
            {
                var internalApiClient = SetupInternalApiClient();

                await internalApiClient.CreateNewTenant(null);
            }
            catch (ApiConnectionException)
            {
                Assert.True(true, "ApiConnectionException is thrown. This behaviour is legal.");
                return;
            }
            
            Assert.False(true, "ApiConnectionException is not thrown. This behaviour is illegal.");
        }

        [Fact]
        public async Task TestingCreateNewTenant_ThrowingApiStatusCodeException_ShouldPass()
        {
            try
            {
                var internalApiClient = SetupInternalApiClientWithInternalServerErrorException();

                await internalApiClient.CreateNewTenant(new Common.Models.ApiContracts.NewTenant
                {
                    Name = "Test company 1",
                    Alias = "test-company-1"
                });
            }
            catch (ApiStatusCodeException)
            {
                Assert.True(true, "ApiStatusCodeException is thrown. This behaviour is legal.");
                return;
            }
            
            Assert.False(true, "ApiStatusCodeException is not thrown. This behaviour is illegal.");
        } 
        
        [Fact]
        public async Task TestingCreateNewTenant_NullResponse_ThrowingApiConnectionException_ShouldPass()
        {
            try
            {
                var internalApiClient = SetupInternalApiClientWithNullResponse();

                await internalApiClient.CreateNewTenant(new Common.Models.ApiContracts.NewTenant
                {
                    Name = "Test company 1",
                    Alias = "test-company-1"
                });
            }
            catch (ApiConnectionException)
            {
                Assert.True(true, "ApiConnectionException is thrown. This behaviour is legal.");
                return;
            }
            
            Assert.False(true, "ApiConnectionException is not thrown. This behaviour is illegal.");
        }

        [Fact]
        public async Task TestingCreateNewTenant_GeneralException_ThrowingApiConnectionException_ShouldPass()
        {
            try
            {
                var internalApiClient = SetupInternalApiClientWithGeneralException();

                await internalApiClient.CreateNewTenant(new Common.Models.ApiContracts.NewTenant
                {
                    Name = "Test company 1",
                    Alias = "test-company-1"
                });
            }
            catch (ApiConnectionException)
            {
                Assert.True(true, "ApiConnectionException is thrown. This behaviour is legal.");
                return;
            }
            
            Assert.False(true, "ApiConnectionException is not thrown. This behaviour is illegal.");
        }

        [Fact]
        public async Task TestingCreateNewTenant_HttpRequestException_ThrowingApiConnectionException_ShouldPass()
        {
            try
            {
                var internalApiClient = SetupInternalApiClientWithHttpRequestException();

                await internalApiClient.CreateNewTenant(new Common.Models.ApiContracts.NewTenant
                {
                    Name = "Test company 1",
                    Alias = "test-company-1"
                });
            }
            catch (ApiConnectionException)
            {
                Assert.True(true, "ApiConnectionException is thrown. This behaviour is legal.");
                return;
            }
            
            Assert.False(true, "ApiConnectionException is not thrown. This behaviour is illegal.");
        }

        [Fact]
        public async Task TestingEditTenant_PassingValidGuidAndTenant_ShouldPass()
        {
            var internalApiClient = SetupInternalApiClient();

            await internalApiClient.EditTenant(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C"), new Common.Models.ApiContracts.Tenant
            {
                Name = "Test company 1",
                Alias = "test-company-1",
                Guid = new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C")
            });
            
            Assert.True(true, "If test completed without exception it means test passed successfully.");
        }
        
        [Fact]
        public async Task TestingEditTenant_PassingNullTenant_ThrowingApiConnectionException_ShouldPass()
        {
            try
            {
                var internalApiClient = SetupInternalApiClient();

                await internalApiClient.EditTenant(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C"), null);
            }
            catch (ApiConnectionException)
            {
                Assert.True(true, "ApiConnectionException is thrown. This behaviour is legal.");
                return;
            }
            
            Assert.False(true, "ApiConnectionException is not thrown. This behaviour is illegal.");
        }
        
        [Fact]
        public async Task TestingEditTenant_PassingInvalidGuid_ThrowingNotFoundException_ShouldPass()
        {
            try
            {
                var internalApiClient = SetupInternalApiClient();

                await internalApiClient.EditTenant(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0BFFF"), new Common.Models.ApiContracts.Tenant
                {
                    Name = "Test company 1",
                    Alias = "test-company-1",
                    Guid = new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0BFFF")
                });
            }
            catch (TenantNotFoundException)
            {
                Assert.True(true, "TenantNotFoundException is thrown. This behaviour is legal.");
                return;
            }
            
            Assert.False(true, "TenantNotFoundException is not thrown. This behaviour is illegal.");
        }        

        [Fact]
        public async Task TestingEditTenant_ThrowingApiStatusCodeException_ShouldPass()
        {
            try
            {
                var internalApiClient = SetupInternalApiClientWithInternalServerErrorException();

                await internalApiClient.EditTenant(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C"), new Common.Models.ApiContracts.Tenant
                {
                    Name = "Test company 1",
                    Alias = "test-company-1",
                    Guid = new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C")
                });
            }
            catch (ApiStatusCodeException)
            {
                Assert.True(true, "ApiStatusCodeException is thrown. This behaviour is legal.");
                return;
            }
            
            Assert.False(true, "ApiStatusCodeException is not thrown. This behaviour is illegal.");
        } 
        
        [Fact]
        public async Task TestingEditTenant_NullResponse_ThrowingApiConnectionException_ShouldPass()
        {
            try
            {
                var internalApiClient = SetupInternalApiClientWithNullResponse();

                await internalApiClient.EditTenant(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C"), new Common.Models.ApiContracts.Tenant
                {
                    Name = "Test company 1",
                    Alias = "test-company-1",
                    Guid = new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C")
                });
            }
            catch (ApiConnectionException)
            {
                Assert.True(true, "ApiConnectionException is thrown. This behaviour is legal.");
                return;
            }
            
            Assert.False(true, "ApiConnectionException is not thrown. This behaviour is illegal.");
        } 
        
        [Fact]
        public async Task TestingEditTenant_GeneralException_ThrowingApiConnectionException_ShouldPass()
        {
            try
            {
                var internalApiClient = SetupInternalApiClientWithGeneralException();

                await internalApiClient.EditTenant(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C"), new Common.Models.ApiContracts.Tenant
                {
                    Name = "Test company 1",
                    Alias = "test-company-1",
                    Guid = new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C")
                });
            }
            catch (ApiConnectionException)
            {
                Assert.True(true, "ApiConnectionException is thrown. This behaviour is legal.");
                return;
            }
            
            Assert.False(true, "ApiConnectionException is not thrown. This behaviour is illegal.");
        } 
        
        [Fact]
        public async Task TestingEditTenant_HttpRequestException_ThrowingApiConnectionException_ShouldPass()
        {
            try
            {
                var internalApiClient = SetupInternalApiClientWithHttpRequestException();

                await internalApiClient.EditTenant(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C"), new Common.Models.ApiContracts.Tenant
                {
                    Name = "Test company 1",
                    Alias = "test-company-1",
                    Guid = new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C")
                });
            }
            catch (ApiConnectionException)
            {
                Assert.True(true, "ApiConnectionException is thrown. This behaviour is legal.");
                return;
            }
            
            Assert.False(true, "ApiConnectionException is not thrown. This behaviour is illegal.");
        }         

        [Fact]
        public async Task TestingDeleteTenant_PassingValidGuid_ShouldPass()
        {
            var internalApiClient = SetupInternalApiClient();

            await internalApiClient.DeleteTenant(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C"));
            
            Assert.True(true, "If test completed without exception it means test passed successfully.");
        }
        
        [Fact]
        public async Task TestingDeleteTenant_PassingInvalidGuid_ThrowingTenantNotFoundException_ShouldPass()
        {
            try
            {
                var internalApiClient = SetupInternalApiClient();

                await internalApiClient.DeleteTenant(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0BFFF"));
            }
            catch (TenantNotFoundException)
            {
                Assert.True(true, "TenantNotFoundException is thrown. This behaviour is legal.");
                return;
            }
            
            Assert.False(true, "TenantNotFoundException is not thrown. This behaviour is illegal.");
        }
        
        [Fact]
        public async Task TestingDeleteTenant_ThrowingApiStatusCodeException_ShouldPass()
        {
            try
            {
                var internalApiClient = SetupInternalApiClientWithInternalServerErrorException();

                await internalApiClient.DeleteTenant(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C"));
            }
            catch (ApiStatusCodeException)
            {
                Assert.True(true, "ApiStatusCodeException is thrown. This behaviour is legal.");
                return;
            }
            
            Assert.False(true, "ApiStatusCodeException is not thrown. This behaviour is illegal.");
        }        
        
        [Fact]
        public async Task TestingDeleteTenant_NullResponse_ThrowingApiConnectionException_ShouldPass()
        {
            try
            {
                var internalApiClient = SetupInternalApiClientWithNullResponse();

                await internalApiClient.DeleteTenant(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C"));
            }
            catch (ApiConnectionException)
            {
                Assert.True(true, "ApiConnectionException is thrown. This behaviour is legal.");
                return;
            }
            
            Assert.False(true, "ApiConnectionException is not thrown. This behaviour is illegal.");
        } 
        
        [Fact]
        public async Task TestingDeleteTenant_GeneralException_ThrowingApiConnectionException_ShouldPass()
        {
            try
            {
                var internalApiClient = SetupInternalApiClientWithGeneralException();

                await internalApiClient.DeleteTenant(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C"));
            }
            catch (ApiConnectionException)
            {
                Assert.True(true, "ApiConnectionException is thrown. This behaviour is legal.");
                return;
            }
            
            Assert.False(true, "ApiConnectionException is not thrown. This behaviour is illegal.");
        }        

        [Fact]
        public async Task TestingDeleteTenant_HttpRequestException_ThrowingApiConnectionException_ShouldPass()
        {
            try
            {
                var internalApiClient = SetupInternalApiClientWithHttpRequestException();

                await internalApiClient.DeleteTenant(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C"));
            }
            catch (ApiConnectionException)
            {
                Assert.True(true, "ApiConnectionException is thrown. This behaviour is legal.");
                return;
            }
            
            Assert.False(true, "ApiConnectionException is not thrown. This behaviour is illegal.");
        }         
        
        [Fact]
        public async Task TestingCheckApiOverallStatus_ShouldPass()
        {
            var internalApiClient = SetupInternalApiClient();

            var result = await internalApiClient.CheckApiOverallStatus();

            Assert.True(result);            
        }
        
        [Fact]
        public async Task TestingCheckApiReadyStatus_ShouldPass()
        {
            var internalApiClient = SetupInternalApiClient();

            var result = await internalApiClient.CheckApiReadyStatus();

            Assert.True(result);            
        }
        
        [Fact]
        public async Task TestingCheckApiLiveStatus_ShouldPass()
        {
            var internalApiClient = SetupInternalApiClient();

            var result = await internalApiClient.CheckApiLiveStatus();

            Assert.True(result);            
        }      
        
        [Fact]
        public async Task TestingCheckApiLiveStatus_ThrowingException_ShouldPass()
        {
            var internalApiClient = SetupInternalApiClientWithGeneralException();

            var result = await internalApiClient.CheckApiLiveStatus();

            Assert.False(result);            
        }
    }
}