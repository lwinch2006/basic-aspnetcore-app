using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Dka.AspNetCore.BasicWebApp.Api.Controllers.Administration;
using Dka.AspNetCore.BasicWebApp.Common.Logic;
using Dka.AspNetCore.BasicWebApp.Common.Models.ApiContracts;
using Dka.AspNetCore.BasicWebApp.Common.Models.ApiContracts.Tenants;
using Dka.AspNetCore.BasicWebApp.Common.Models.Configurations;
using Dka.AspNetCore.BasicWebApp.Common.Models.ExceptionProcessing;
using Dka.AspNetCore.BasicWebApp.Common.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Tenant = Dka.AspNetCore.BasicWebApp.Common.Models.Tenants.Tenant;

namespace Dka.AspNetCore.BasicWebApp.Api.UnitTests.Controllers.Administration
{
    public class TenantControllerTests
    {
        private TenantsController SetupTenantController()
        {
            var logger = new Mock<ILogger<TenantsController>>();
            var mapperConfig = new MapperConfiguration(cfg => {
                cfg.CreateMap<NewTenantContract, Tenant>();
                cfg.CreateMap<Tenant, NewTenantContract>();
                cfg.CreateMap<TenantContract, Tenant>();
                cfg.CreateMap<Tenant, TenantContract>();
                cfg.CreateMap<EditTenantContract, Tenant>().ReverseMap();
            });
            
            var mapper = mapperConfig.CreateMapper();
            var databaseConfiguration = new Mock<DatabaseConfiguration>();
            var databaseConnectionFactory = new Mock<DatabaseConnectionFactory>(databaseConfiguration.Object);
            var tenantRepository = new Mock<TenantRepository>(databaseConnectionFactory.Object);
            var tenantLogic = new Mock<TenantLogic>(tenantRepository.Object);
            tenantLogic.Setup(logic => logic.GetAll()).Returns(Tenant.GetDummyTenantSet());
            tenantLogic.Setup(logic => logic.GetByGuid(It.IsAny<Guid>())).Returns<Guid>(guid => Task.FromResult(Tenant.GetDummyTenantSet().GetAwaiter().GetResult().ToList().FirstOrDefault(record => record.Guid == guid)));
            tenantLogic.Setup(logic => logic.DeleteTenant(It.Is<Guid>(value => value != new Guid("F02E8F1F-0BBA-4049-9ED6-902F610DEE95")))).Returns<Guid>(guid =>
            {
                var tenantList = Tenant.GetDummyTenantSet().GetAwaiter().GetResult().ToList();

                if (tenantList.Find(record => record.Guid == guid) is { } foundTenant)
                {
                    tenantList.Remove(foundTenant);
                    return Task.FromResult(1);
                }

                return Task.FromResult(0);
            });
            
            tenantLogic.Setup(logic => logic.DeleteTenant(It.Is<Guid>(value => value == new Guid("F02E8F1F-0BBA-4049-9ED6-902F610DEE95")))).Returns<Guid>(guid => throw new BasicWebAppException());
            
            tenantLogic.Setup(logic => logic.CreateNewTenant(It.IsAny<Tenant>())).Returns<Tenant>(tenant =>
            {
                var tenantList = Tenant.GetDummyTenantSet().GetAwaiter().GetResult().ToList();

                if (tenantList.Find(record => record.Name.Equals(tenant.Name, StringComparison.OrdinalIgnoreCase)) is { })
                {
                    throw new BasicWebAppException();
                }

                tenant.Guid = new Guid("DE5BC94F-80E7-44AB-B1EF-BDFF7E47C2D6");
                tenant.Id = tenantList.Max(record => record.Id) + 1;

                tenantList.Add(tenant);
                
                return Task.FromResult(tenantList[^1].Guid);
            });

            tenantLogic.Setup(logic => logic.EditTenant(It.Is<Tenant>(tenant => new [] { new Guid("DE5BC94F-80E7-44AB-B1EF-BDFF7E47CFFF"), new Guid("F02E8F1F-0BBA-4049-9ED6-902F610DEE95") }.Any(guid => tenant.Guid != guid)))).Returns<Tenant>(tenant =>
            {
                var tenantList = Tenant.GetDummyTenantSet().GetAwaiter().GetResult().ToList();
                
                if (tenantList.Find(record => record.Guid == tenant.Guid) is { } foundTenant)
                {
                    foundTenant = tenant;
                    return Task.FromResult(1);
                }

                return Task.FromResult(0);
            });
            
            tenantLogic.Setup(logic => logic.EditTenant(It.Is<Tenant>(tenant => new [] { new Guid("DE5BC94F-80E7-44AB-B1EF-BDFF7E47CFFF"), new Guid("F02E8F1F-0BBA-4049-9ED6-902F610DEE95") }.Any(guid => tenant.Guid == guid)))).Returns<Tenant>(tenant => throw new BasicWebAppException());            
            
            var tenantController = new TenantsController(tenantLogic.Object, mapper, logger.Object);

            return tenantController;
        }

        [Fact]
        public async Task GetAllTenants_CheckResultTypeModelAndData_ShouldPass()
        {
            var tenantController = SetupTenantController();

            var result = await tenantController.GetAll();

            var apiResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<TenantContract>>(apiResult.Value).ToList();
            
            Assert.Equal(3, model.Count);
            Assert.Contains(model, record => record.Name.Equals("Umbrella Corporation", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public async Task GetTenantByGuid_PassValidGuid_ShouldPass()
        {
            var tenantController = SetupTenantController();

            var result = await tenantController.GetByGuid(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C"));

            var apiResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<TenantContract>(apiResult.Value);

            Assert.NotNull(model);
            Assert.Equal("Umbrella Corporation", model.Name);
        }
        
        [Fact]
        public async Task GetTenantByGuid_PassInvalidGuid_ShouldPass()
        {
            var tenantController = SetupTenantController();

            var result = await tenantController.GetByGuid(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0BFFF"));

            var apiResult = Assert.IsType<NotFoundResult>(result);

            Assert.Equal((int)HttpStatusCode.NotFound, apiResult.StatusCode);
        }

        [Fact]
        public async Task DeleteTenant_PassInvalidGuid_ShouldPass()
        {
            var tenantController = SetupTenantController();

            var result = await tenantController.DeleteTenant(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0BFFF"));

            var apiResult = Assert.IsType<NotFoundResult>(result);
            
            Assert.Equal(StatusCodes.Status404NotFound, apiResult.StatusCode);
        }

        [Fact]
        public async Task DeleteTenant_PassValidGuid_ShouldPass()
        {
            var tenantController = SetupTenantController();

            var result = await tenantController.DeleteTenant(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C"));

            var apiResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<Tenant>(apiResult.Value);

            Assert.Equal(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C"), model.Guid);
        }

        [Fact]
        public async Task DeleteTenant_ThrowBasicWebException_ShouldPass()
        {
            var tenantController = SetupTenantController();

            var result = await tenantController.DeleteTenant(new Guid("F02E8F1F-0BBA-4049-9ED6-902F610DEE95"));

            var apiResult = Assert.IsType<StatusCodeResult>(result);
            
            Assert.Equal(StatusCodes.Status500InternalServerError, apiResult.StatusCode);
        }

        [Fact]
        public async Task CreateNewTenant_ProvideCorrectData_ShouldPass()
        {
            var tenantController = SetupTenantController();
            
            var result = await tenantController.CreateNewTenant(new NewTenantContract { Name = "Test Company", Alias = "test-company" });

            var apiResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<Guid>(apiResult.Value);

            Assert.Equal(new Guid("DE5BC94F-80E7-44AB-B1EF-BDFF7E47C2D6"), model);
        }

        [Fact]
        public async Task CreateNewTenant_ThrowExceptionWhenTwoTenantsWithSameIdProvided_ShouldPass()
        {
            var tenantController = SetupTenantController();
            
            var result = await tenantController.CreateNewTenant(new NewTenantContract { Name = "Umbrella Corporation", Alias = "umbrella" });

            var apiResult = Assert.IsType<StatusCodeResult>(result);
            
            Assert.Equal(StatusCodes.Status500InternalServerError, apiResult.StatusCode);
        }

        [Fact]
        public async Task CreateNewTenant_PassNullParameterReturnBadRequestResponse_ShouldPass()
        {
            var tenantController = SetupTenantController();
            
            var result = await tenantController.CreateNewTenant(null);
            
            var apiResult = Assert.IsType<BadRequestResult>(result);
            
            Assert.Equal(StatusCodes.Status400BadRequest, apiResult.StatusCode);            
        }

        [Fact]
        public async Task EditTenant_PassDifferentGuids_ReturnsBadRequest_ShouldPass()
        {
            var tenantController = SetupTenantController();
            
            var result = await tenantController.EditTenant(new Guid("DE5BC94F-80E7-44AB-B1EF-BDFF7E47CFFF"), new EditTenantContract { Name = "Test Company", Alias = "test-company" });

            var apiResult = Assert.IsType<NotFoundResult>(result);
            
            Assert.Equal(StatusCodes.Status404NotFound, apiResult.StatusCode);  
        }

        [Fact]
        public async Task EditTenant_PassNullTenantAndInvalidGuid_ReturnsBadRequest_ShouldPass()
        {
            var tenantController = SetupTenantController();
            var result = await tenantController.EditTenant(new Guid("DE5BC94F-80E7-44AB-B1EF-BDFF7E47CFFF"), null);

            var apiResult = Assert.IsType<NotFoundResult>(result);

            Assert.Equal(StatusCodes.Status404NotFound, apiResult.StatusCode);
        }

        [Fact]
        public async Task EditTenant_PassValidTenantAndGuid_ReturnsNoContent_ShouldPass()
        {
            var tenantController = SetupTenantController();
            var result = await tenantController.EditTenant(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C"), new EditTenantContract { Name = "Umbrella Corporation1", Alias = "umbrella1" });
            
            var apiResult = Assert.IsType<NoContentResult>(result);
            
            Assert.Equal(StatusCodes.Status204NoContent, apiResult.StatusCode);
        }
        
        [Fact]
        public async Task EditTenant_PassValidTenantAndGuid_ThrowsException_ReturnsNotFound_ShouldPass()
        {
            var tenantController = SetupTenantController();
            var result = await tenantController.EditTenant(new Guid("DE5BC94F-80E7-44AB-B1EF-BDFF7E47CFFF"), new EditTenantContract { Name = "Umbrella Corporation1", Alias = "umbrella1" });
            var apiResult = Assert.IsType<NotFoundResult>(result);
            
            Assert.Equal(StatusCodes.Status404NotFound, apiResult.StatusCode);
        }
        
        [Fact]
        public async Task EditTenant_PassValidTenantAndGuid_ThrowsException_ReturnsInternalServerError_ShouldPass()
        {
            var tenantController = SetupTenantController();
            var result = await tenantController.EditTenant(new Guid("F02E8F1F-0BBA-4049-9ED6-902F610DEE95"), new EditTenantContract { Name = "Cyberdyne Systems1", Alias = "cyberdyne1" });
            var apiResult = Assert.IsType<StatusCodeResult>(result);
            
            Assert.Equal(StatusCodes.Status500InternalServerError, apiResult.StatusCode);
        }
    }
}