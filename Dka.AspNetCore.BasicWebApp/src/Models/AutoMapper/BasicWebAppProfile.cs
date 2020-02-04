using AutoMapper;
using Dka.AspNetCore.BasicWebApp.Models.ExceptionProcessing;

namespace Dka.AspNetCore.BasicWebApp.Models.AutoMapper
{
    public class BasicWebAppProfile : Profile
    {
        public BasicWebAppProfile()
        {
            // Logic model -> API contract.
            CreateMap<Common.Models.Tenants.Tenant, Common.Models.ApiContracts.Tenants.TenantContract>();
            CreateMap<Common.Models.Tenants.Tenant, Common.Models.ApiContracts.Tenants.NewTenantContract>();
            CreateMap<Common.Models.Tenants.Tenant, Common.Models.ApiContracts.Tenants.EditTenantContract>();

            // Logic model -> ViewModel.
            CreateMap<Common.Models.Tenants.Tenant, ViewModels.Tenants.TenantViewModel>();
            CreateMap<Common.Models.Tenants.Tenant, ViewModels.Tenants.NewTenantViewModel>();
            CreateMap<Common.Models.Tenants.Tenant, ViewModels.Tenants.EditTenantViewModel>();
            CreateMap<WebAppStatusCodeException, ViewModels.ExceptionProcessing.ErrorViewModel>();
            
            // API contract -> Logic model.
            CreateMap<Common.Models.ApiContracts.Tenants.TenantContract, Common.Models.Tenants.Tenant>();
            CreateMap<Common.Models.ApiContracts.Tenants.NewTenantContract, Common.Models.Tenants.Tenant>();
            CreateMap<Common.Models.ApiContracts.Tenants.EditTenantContract, Common.Models.Tenants.Tenant>();
            
            // API contract -> ViewModel.
            CreateMap<Common.Models.ApiContracts.Tenants.TenantContract, ViewModels.Tenants.TenantViewModel>();
            CreateMap<Common.Models.ApiContracts.Tenants.TenantContract, ViewModels.Tenants.EditTenantViewModel>();
            CreateMap<Common.Models.ApiContracts.Tenants.NewTenantContract, ViewModels.Tenants.NewTenantViewModel>();
            CreateMap<Common.Models.ApiContracts.Tenants.EditTenantContract, ViewModels.Tenants.EditTenantViewModel>();
            CreateMap<Common.Models.ApiContracts.Authentication.SignInRequestContract, ViewModels.Authentication.SignInViewModel>();
            
            // ViewModel -> API contract.
            CreateMap<ViewModels.Tenants.TenantViewModel, Common.Models.ApiContracts.Tenants.TenantContract>();
            CreateMap<ViewModels.Tenants.TenantViewModel, Common.Models.ApiContracts.Tenants.EditTenantContract>();
            CreateMap<ViewModels.Tenants.NewTenantViewModel, Common.Models.ApiContracts.Tenants.NewTenantContract>();
            CreateMap<ViewModels.Tenants.EditTenantViewModel, Common.Models.ApiContracts.Tenants.EditTenantContract>();
            CreateMap<ViewModels.Authentication.SignInViewModel, Common.Models.ApiContracts.Authentication.SignInRequestContract>();
            
            // ViewModel -> Logic model.
            CreateMap<ViewModels.Tenants.TenantViewModel, Common.Models.Tenants.Tenant>();
            CreateMap<ViewModels.Tenants.NewTenantViewModel, Common.Models.Tenants.Tenant>();
            CreateMap<ViewModels.Tenants.EditTenantViewModel, Common.Models.Tenants.Tenant>();
            CreateMap<ViewModels.ExceptionProcessing.ErrorViewModel, WebAppStatusCodeException>();            
        }
    }
}