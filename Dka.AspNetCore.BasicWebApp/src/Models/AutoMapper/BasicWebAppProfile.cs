using AutoMapper;

namespace Dka.AspNetCore.BasicWebApp.Models.AutoMapper
{
    public class BasicWebAppProfile : Profile
    {
        public BasicWebAppProfile()
        {
            // Logic model -> API contract.
            CreateMap<Common.Models.Tenants.Tenant, Common.Models.ApiContracts.Tenant>();
            CreateMap<Common.Models.Tenants.Tenant, Common.Models.ApiContracts.NewTenant>();
            
            // Logic model -> ViewModel.
            CreateMap<Common.Models.Tenants.Tenant, ViewModels.Tenants.Tenant>();
            CreateMap<Common.Models.Tenants.Tenant, ViewModels.Tenants.NewTenant>();
            
            // API contract -> Logic model.
            CreateMap<Common.Models.ApiContracts.Tenant, Common.Models.Tenants.Tenant>();
            CreateMap<Common.Models.ApiContracts.NewTenant, Common.Models.Tenants.Tenant>();
            
            // API contract -> ViewModel.
            CreateMap<Common.Models.ApiContracts.Tenant, ViewModels.Tenants.Tenant>();
            CreateMap<Common.Models.ApiContracts.NewTenant, ViewModels.Tenants.NewTenant>();
            CreateMap<Common.Models.ApiContracts.Authentication.SignInRequestContract, ViewModels.Authentication.SignInViewModel>();
            
            // ViewModel -> API contract.
            CreateMap<ViewModels.Tenants.Tenant, Common.Models.ApiContracts.Tenant>();
            CreateMap<ViewModels.Tenants.NewTenant, Common.Models.ApiContracts.NewTenant>();
            CreateMap<ViewModels.Authentication.SignInViewModel, Common.Models.ApiContracts.Authentication.SignInRequestContract>();
            
            // ViewModel -> Logic model.
            CreateMap<ViewModels.Tenants.Tenant, Common.Models.Tenants.Tenant>();
            CreateMap<ViewModels.Tenants.NewTenant, Common.Models.Tenants.Tenant>();
        }
    }
}