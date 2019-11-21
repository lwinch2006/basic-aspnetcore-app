using AutoMapper;

namespace Dka.AspNetCore.BasicWebApp.Api.Models.AutoMapper
{
    public class BasicWebAppApiProfile : Profile
    {
        public BasicWebAppApiProfile()
        {
            // Logic model -> API contract.
            CreateMap<Common.Models.Tenants.Tenant, Common.Models.ApiContracts.Tenant>();
            CreateMap<Common.Models.Tenants.Tenant, Common.Models.ApiContracts.NewTenant>();
            
            
            // API contract -> Logic model.
            CreateMap<Common.Models.ApiContracts.Tenant, Common.Models.Tenants.Tenant>();
            CreateMap<Common.Models.ApiContracts.NewTenant, Common.Models.Tenants.Tenant>();
        }
    }
}