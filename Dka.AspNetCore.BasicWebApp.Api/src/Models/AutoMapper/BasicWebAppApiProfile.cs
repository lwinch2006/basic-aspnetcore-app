using AutoMapper;

namespace Dka.AspNetCore.BasicWebApp.Api.Models.AutoMapper
{
    public class BasicWebAppApiProfile : Profile
    {
        public BasicWebAppApiProfile()
        {
            // Logic model -> API contract.
            CreateMap<Common.Models.Tenants.Tenant, Common.Models.ApiContracts.Tenants.TenantContract>();
            CreateMap<Common.Models.Tenants.Tenant, Common.Models.ApiContracts.Tenants.NewTenantContract>();
            
            
            // API contract -> Logic model.
            CreateMap<Common.Models.ApiContracts.Tenants.TenantContract, Common.Models.Tenants.Tenant>();
            CreateMap<Common.Models.ApiContracts.Tenants.NewTenantContract, Common.Models.Tenants.Tenant>();
        }
    }
}