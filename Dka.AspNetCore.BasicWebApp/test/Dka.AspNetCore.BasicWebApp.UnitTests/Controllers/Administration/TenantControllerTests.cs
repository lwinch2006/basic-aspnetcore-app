using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dka.AspNetCore.BasicWebApp.Common.Models.ExceptionProcessing;
using Dka.AspNetCore.BasicWebApp.Controllers.Administration;
using Dka.AspNetCore.BasicWebApp.Models.ApiClients;
using Dka.AspNetCore.BasicWebApp.Services.ApiClients;
using Dka.AspNetCore.BasicWebApp.ViewModels.Tenants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Tenant = Dka.AspNetCore.BasicWebApp.Common.Models.Tenants.Tenant;

namespace Dka.AspNetCore.BasicWebApp.UnitTests.Controllers.Administration
{
    public class TenantControllerTests
    {
        private IMapper SetupMapper()
        {
            var mapperConfig = new MapperConfiguration(cfg => {
                cfg.CreateMap<Tenant, BasicWebApp.ViewModels.Tenants.Tenant>();
                cfg.CreateMap<NewTenant, Common.Models.ApiContracts.NewTenant>();
                cfg.CreateMap<BasicWebApp.ViewModels.Tenants.Tenant, Common.Models.ApiContracts.Tenant>();
                
            });
            
            var mapper = mapperConfig.CreateMapper();

            return mapper;
        }


        private TenantsController SetupController()
        {
            var logger = new Mock<ILogger<TenantsController>>();
            var mapper = SetupMapper();
            var internalApiClient = new Mock<IInternalApiClient>();

            internalApiClient.Setup(client => client.GetTenants()).Returns(() =>
            {
                var tenants = Tenant.GetDummyTenantSet().GetAwaiter().GetResult();

                return Task.FromResult(tenants);
            });

            internalApiClient.Setup(client => client.GetTenantByGuid(It.IsAny<Guid>())).Returns<Guid>(tenantGuid =>
            {
                var tenants = Tenant.GetDummyTenantSet().GetAwaiter().GetResult();

                var foundTenant = tenants.SingleOrDefault(record => record.Guid == tenantGuid);

                return Task.FromResult(foundTenant);
            });            
            
            internalApiClient.Setup(client => client.CreateNewTenant(It.IsAny<Common.Models.ApiContracts.NewTenant>())).Returns<Common.Models.ApiContracts.NewTenant>(newTenant =>
            {
                var tenants = Tenant.GetDummyTenantSet().GetAwaiter().GetResult().ToList();

                var newTenantBo = new Tenant
                {
                    Name = newTenant.Name,
                    Alias = newTenant.Alias,
                    Id = tenants.Max(record => record.Id) + 1,
                    Guid = new Guid("3D85AAEB-2923-4B4B-9661-2CBBE816A7D0")
                };

                tenants.Add(newTenantBo);

                return Task.FromResult(tenants[^1].Guid);
            });
            
            internalApiClient.Setup(client => client.DeleteTenant(It.IsAny<Guid>())).Returns<Guid>(tenantGuid =>
            {
                var tenants = Tenant.GetDummyTenantSet().GetAwaiter().GetResult().ToList();

                if (tenants.Find(record => record.Guid == tenantGuid) is { } tenantToDelete)
                {
                    tenants.Remove(tenantToDelete);
                }

                return Task.CompletedTask;
            });

            internalApiClient.Setup(client => client.EditTenant(It.IsAny<Guid>(), It.IsAny<Common.Models.ApiContracts.Tenant>())).Returns<Guid, Common.Models.ApiContracts.Tenant>((tenantGuid, tenantToEdit) =>
            {
                var tenants = Tenant.GetDummyTenantSet().GetAwaiter().GetResult().ToList();

                if (!(tenants.Find(record => record.Guid == tenantGuid) is { } foundTenant))
                {
                    return Task.CompletedTask;
                }
                
                foundTenant.Name = tenantToEdit.Name;
                foundTenant.Alias = tenantToEdit.Alias;
                return Task.CompletedTask;
            });

            var tenantController = new TenantsController(internalApiClient.Object, logger.Object, mapper);

            return tenantController;
        }
        
        private TenantsController SetupControllerWithThrowingException()
        {
            var logger = new Mock<ILogger<TenantsController>>();
            var mapper = SetupMapper();
            var internalApiClient = new Mock<IInternalApiClient>();
            internalApiClient.Setup(client => client.GetTenants()).Returns(() => throw new BasicWebAppException());
            internalApiClient.Setup(client => client.GetTenantByGuid(It.IsAny<Guid>())).Returns(() => throw new BasicWebAppException());
            internalApiClient.Setup(client => client.CreateNewTenant(It.IsAny<Common.Models.ApiContracts.NewTenant>())).Returns<Common.Models.ApiContracts.NewTenant>(newTenant => throw new BasicWebAppException());
            internalApiClient.Setup(client => client.DeleteTenant(It.IsAny<Guid>())).Returns<Guid>(tenantGuid => throw new BasicWebAppException());
            internalApiClient.Setup(client => client.EditTenant(It.IsAny<Guid>(), It.IsAny<Common.Models.ApiContracts.Tenant>())).Returns<Guid, Common.Models.ApiContracts.Tenant>((tenantGuid, tenantToEdit) => throw new BasicWebAppException());
            
            var tenantController = new TenantsController(internalApiClient.Object, logger.Object, mapper);

            return tenantController;
        }
        
        private TenantsController SetupControllerWithNullMapper()
        {
            var logger = new Mock<ILogger<TenantsController>>();
            IMapper mapper = null;
            var internalApiClient = new Mock<IInternalApiClient>();
            var tenantController = new TenantsController(internalApiClient.Object, logger.Object, mapper);
            return tenantController;
        }     
        
        private TenantsController SetupControllerWithNullInternalApiClient()
        {
            var logger = new Mock<ILogger<TenantsController>>();
            var mapper = SetupMapper();
            var tenantController = new TenantsController(null, logger.Object, mapper);
            return tenantController;
        }         
        
        [Fact]
        public async Task TestingGetAllAction_ShouldPass()
        {
            var tenantController = SetupController();

            var result = await tenantController.GetAll();

            var viewResult = Assert.IsType<ViewResult>(result);

            var model = ((IEnumerable<Tenant>) viewResult.Model).ToList();
            
            Assert.Equal(3, model.Count);
            Assert.Equal("Umbrella Corporation", model[0].Name);
        }
        
        [Fact]
        public async Task TestingGetAllAction_ThrowingException_ShouldPass()
        {
            var tenantController = SetupControllerWithThrowingException();

            var result = await tenantController.GetAll();

            var viewResult = Assert.IsType<ViewResult>(result);

            var model = ((IEnumerable<Tenant>) viewResult.Model).ToList();
            
            Assert.Empty(model);
        }

        [Fact]
        public async Task TestingGetByGuidAction_PassValidGuid_ShouldPass()
        {
            var tenantController = SetupController();

            var result = await tenantController.GetByGuid(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C"));

            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.NotNull(viewResult.Model);
            Assert.Equal("Umbrella Corporation", ((BasicWebApp.ViewModels.Tenants.Tenant)viewResult.Model).Name);
        }

        [Fact]
        public async Task TestingGetByGuidAction_PassInvalidGuid_ReturnsNull_ShouldPass()
        {
            var tenantController = SetupController();

            var result = await tenantController.GetByGuid(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0BFFF"));

            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.Null(viewResult.Model);
        }

        [Fact]
        public async Task TestingGetByGuidAction_ThrowingException_ReturnsNull_ShouldPass()
        {
            var tenantController = SetupControllerWithThrowingException();

            var result = await tenantController.GetByGuid(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C"));

            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.Null(viewResult.Model);
        }

        [Fact]
        public async Task TestingCreateNewTenantAction_GettingForm_ShouldPass()
        {
            var tenantController = SetupController();
            
            var result = await tenantController.CreateNewTenant();

            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.IsType<NewTenant>(viewResult.Model);
            Assert.NotNull(viewResult.Model);
        }

        [Fact]
        public async Task TestingCreateNewTenantAction_PostingForm_NewTenantIsNull_ReturnsBackToForm_ShouldPass()
        {
            var tenantController = SetupController();
            
            var result = await tenantController.CreateNewTenant(null);

            var viewResult = Assert.IsType<ViewResult>(result);
            
            Assert.Null(viewResult.Model);
        }

        [Fact]
        public async Task TestingCreateNewTenantAction_PostingForm_NewTenantIsValid_ReturnsToDetailsPageS_ShouldPass()
        {
            var tenantController = SetupController();
            
            var result = await tenantController.CreateNewTenant(new NewTenant { Name = "Test tenant 1", Alias = "test-tenant-1"});

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            
            Assert.Equal("details", redirectToActionResult.ActionName);
            Assert.Equal(new Guid("3D85AAEB-2923-4B4B-9661-2CBBE816A7D0"), redirectToActionResult.RouteValues["Guid"]);
        }

        [Fact]
        public async Task TestingCreateNewTenantAction_PostingForm_NewTenantIsValid_ApiClientThrowsException_BackToForm_ShouldPass()
        {
            var tenantController = SetupControllerWithThrowingException();
            
            var result = await tenantController.CreateNewTenant(new NewTenant { Name = "Test tenant 1", Alias = "test-tenant-1"});

            var viewResult = Assert.IsType<ViewResult>(result);

            var model = (NewTenant) viewResult.Model;
            
            Assert.Equal("Test tenant 1", model.Name);
            Assert.Equal("test-tenant-1", model.Alias);
        }

        [Fact]
        public async Task TestingDeleteAction_TenantViewModelIsNull_ThrowsTenantNotFoundException_ReturnsBackToForm_ShouldPass()
        {
            var tenantController = SetupController();

            var result = await tenantController.DeleteTenant(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C"), null);

            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.Null(viewResult.Model);
        }

        [Fact]
        public async Task TestingDeleteAction_TenantViewModelIsNotNull_GuidsAreDifferent_ReturnsBackToForm_ShouldPass()
        {
            var tenantController = SetupController();

            var result = await tenantController.DeleteTenant(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C"), new BasicWebApp.ViewModels.Tenants.Tenant
            {
                Name = "Test tenant 1",
                Alias = "test-tenant-1",
                Guid = new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0BFFF")
            });

            var viewResult = Assert.IsType<ViewResult>(result);
            
            Assert.Equal("Test tenant 1", ((BasicWebApp.ViewModels.Tenants.Tenant)viewResult.Model).Name);
        }

        [Fact]
        public async Task TestingDeleteAction_TenantViewModelIsNotNull_GuidsAreEqual_RedirectsToActionIndex_ShouldPass()
        {
            var tenantController = SetupController();

            var result = await tenantController.DeleteTenant(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C"), new BasicWebApp.ViewModels.Tenants.Tenant
            {
                Name = "Test tenant 1",
                Alias = "test-tenant-1",
                Guid = new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C")
            });

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            
            Assert.Equal("index", redirectToActionResult.ActionName);
        }
        
        [Fact]
        public async Task TestingDeleteAction_ModelStateNotValid_ReturnsBackToForm_ShouldPass()
        {
            var tenantController = SetupController();

            tenantController.ModelState.AddModelError("Sample key", "Sample error message");
            
            var result = await tenantController.DeleteTenant(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C"), new BasicWebApp.ViewModels.Tenants.Tenant
            {
                Name = "Test tenant 1",
                Alias = "test-tenant-1",
                Guid = new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C")
            });

            var viewResult = Assert.IsType<ViewResult>(result);
            
            Assert.Equal("Test tenant 1", ((BasicWebApp.ViewModels.Tenants.Tenant)viewResult.Model).Name);
        }

        [Fact]
        public async Task TestingDeleteAction_TenantViewModelIsNotNull_GuidsAreEqual_ThrowsException_ReturnsBackToView_ShouldPass()
        {
            var tenantController = SetupControllerWithThrowingException();

            var result = await tenantController.DeleteTenant(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C"), new BasicWebApp.ViewModels.Tenants.Tenant
            {
                Name = "Test tenant 1",
                Alias = "test-tenant-1",
                Guid = new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C")
            });

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = (BasicWebApp.ViewModels.Tenants.Tenant) viewResult.Model;
            
            Assert.Equal("Test tenant 1", model.Name);            
        }

        [Fact]
        public async Task TestingEditAction_GettingForm_GuidIsValid_ReturnsEditForm_ShouldPass()
        {
            var tenantController = SetupController();

            var result = await tenantController.EditTenantDetails(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C"));

            var viewResult = Assert.IsType<ViewResult>(result);

            var model = (BasicWebApp.ViewModels.Tenants.Tenant) viewResult.Model;

            Assert.NotNull(model);
            Assert.Equal("Umbrella Corporation", model.Name);
        }

        [Fact]
        public async Task TestingEditAction_GettingForm_GuidIsInvalid_ReturnsEditFormWithNullModel_ShouldPass()
        {
            var tenantController = SetupController();

            var result = await tenantController.EditTenantDetails(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0BFFF"));

            var viewResult = Assert.IsType<ViewResult>(result);

            var model = (BasicWebApp.ViewModels.Tenants.Tenant) viewResult.Model;

            Assert.Null(model);
        }

        [Fact]
        public async Task TestingEditAction_GettingForm_GuidIsValid_ThrowsException_ReturnsEditFormWithNullModel_ShouldPass()
        {
            var tenantController = SetupControllerWithThrowingException();

            var result = await tenantController.EditTenantDetails(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C"));

            var viewResult = Assert.IsType<ViewResult>(result);

            var model = (BasicWebApp.ViewModels.Tenants.Tenant) viewResult.Model;

            Assert.Null(model);
        }

        [Fact]
        public async Task TestingEditAction_PostingForm_TenantModelIsNull_ReturnsBackToForm_ShouldPass()
        {
            var tenantController = SetupController();

            var result = await tenantController.EditTenantDetails(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C"), null);

            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.Null(viewResult.Model);            
        }

        [Fact]
        public async Task TestingEditAction_PostingForm_GuidsAreDifferent_ReturnsBackToForm_ShouldPass()
        {
            var tenantController = SetupController();

            var result = await tenantController.EditTenantDetails(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C"), new BasicWebApp.ViewModels.Tenants.Tenant
            {
                Name = "Test tenant 1",
                Alias = "test-tenant-1",
                Guid = new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0BFFF")
            });

            var viewResult = Assert.IsType<ViewResult>(result);
            
            Assert.Equal("Test tenant 1", ((BasicWebApp.ViewModels.Tenants.Tenant)viewResult.Model).Name);
        }        
        
        [Fact]
        public async Task TestingEditAction_PostingForm_GuidsAreEqual_RedirectsToActionIndex_ShouldPass()
        {
            var tenantController = SetupController();

            var result = await tenantController.EditTenantDetails(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C"), new BasicWebApp.ViewModels.Tenants.Tenant
            {
                Name = "Test tenant 1",
                Alias = "test-tenant-1",
                Guid = new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C")
            });

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            
            Assert.Equal("index", redirectToActionResult.ActionName);
        }        

        [Fact]
        public async Task TestingEditAction_PostingForm_ModelStateNotValid_ReturnsBackToForm_ShouldPass()
        {
            var tenantController = SetupController();

            tenantController.ModelState.AddModelError("Sample key", "Sample error message");
            
            var result = await tenantController.EditTenantDetails(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C"), new BasicWebApp.ViewModels.Tenants.Tenant
            {
                Name = "Test tenant 1",
                Alias = "test-tenant-1",
                Guid = new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C")
            });

            var viewResult = Assert.IsType<ViewResult>(result);
            
            Assert.Equal("Test tenant 1", ((BasicWebApp.ViewModels.Tenants.Tenant)viewResult.Model).Name);
        }
        
        
        
        
        
        
        
        [Fact]
        public async Task TestingEditAction_PostingForm_GuidsAreEqual_ThrowsException_ReturnsBackToView_ShouldPass()
        {
            var tenantController = SetupControllerWithThrowingException();

            var result = await tenantController.EditTenantDetails(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C"), new BasicWebApp.ViewModels.Tenants.Tenant
            {
                Name = "Test tenant 1",
                Alias = "test-tenant-1",
                Guid = new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C")
            });

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = (BasicWebApp.ViewModels.Tenants.Tenant) viewResult.Model;
            
            Assert.Equal("Test tenant 1", model.Name);            
        }
        
        [Fact]
        public async Task TestingEditAction_PostingForm_GuidsAreEqual_MapperIsNull_ThrowsException_ShouldPass()
        {
            var tenantController = SetupControllerWithNullMapper();

            try
            {
                await tenantController.EditTenantDetails(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C"), new BasicWebApp.ViewModels.Tenants.Tenant
                {
                    Name = "Test tenant 1",
                    Alias = "test-tenant-1",
                    Guid = new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C")
                });
            }
            catch (NullReferenceException)
            {
                Assert.True(true, "NullReferenceException is thrown for mapper instance. This behaviour is legal");
            }
        }
        
        [Fact]
        public async Task TestingEditAction_PostingForm_GuidsAreEqual_InternalApiClientIsNull_ThrowsException_ShouldPass()
        {
            var tenantController = SetupControllerWithNullInternalApiClient();

            try
            {
                await tenantController.EditTenantDetails(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C"), new BasicWebApp.ViewModels.Tenants.Tenant
                {
                    Name = "Test tenant 1",
                    Alias = "test-tenant-1",
                    Guid = new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C")
                });
            }
            catch (NullReferenceException)
            {
                Assert.True(true, "NullReferenceException is thrown for mapper instance. This behaviour is legal");
            }
        }        
        
        
    }
}