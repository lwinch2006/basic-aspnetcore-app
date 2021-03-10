using AutoMapper;
using Dka.AspNetCore.BasicWebApp.Areas.Administration.ViewModels.Tenants;
using Dka.AspNetCore.BasicWebApp.Common.Logic.AutoMapper;
using Dka.AspNetCore.BasicWebApp.Common.Models.Pagination;
using Dka.AspNetCore.BasicWebApp.Models.ExceptionProcessing;
using Dka.AspNetCore.BasicWebApp.Services.AutoMapper;
using Dka.AspNetCore.BasicWebApp.ViewModels.Pagination;

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
            CreateMap<Common.Models.Tenants.Tenant, TenantViewModel>();
            CreateMap<Common.Models.Tenants.Tenant, NewTenantViewModel>();
            CreateMap<Common.Models.Tenants.Tenant, EditTenantViewModel>();
            CreateMap<WebAppStatusCodeException, ViewModels.ExceptionProcessing.ErrorViewModel>();
            
            // API contract -> Logic model.
            CreateMap<Common.Models.ApiContracts.Tenants.TenantContract, Common.Models.Tenants.Tenant>();
            CreateMap<Common.Models.ApiContracts.Tenants.NewTenantContract, Common.Models.Tenants.Tenant>();
            CreateMap<Common.Models.ApiContracts.Tenants.EditTenantContract, Common.Models.Tenants.Tenant>();
            
            // API contract -> ViewModel.
            CreateMap<Common.Models.ApiContracts.Tenants.TenantContract, TenantViewModel>();
            CreateMap<Common.Models.ApiContracts.Tenants.TenantContract, EditTenantViewModel>();
            CreateMap<Common.Models.ApiContracts.Tenants.NewTenantContract, NewTenantViewModel>();
            CreateMap<Common.Models.ApiContracts.Tenants.EditTenantContract, EditTenantViewModel>();
            CreateMap<Common.Models.ApiContracts.Authentication.SignInRequestContract, ViewModels.Authentication.SignInViewModel>();
            
            // ViewModel -> API contract.
            CreateMap<TenantViewModel, Common.Models.ApiContracts.Tenants.TenantContract>();
            CreateMap<TenantViewModel, Common.Models.ApiContracts.Tenants.EditTenantContract>();
            CreateMap<NewTenantViewModel, Common.Models.ApiContracts.Tenants.NewTenantContract>();
            CreateMap<EditTenantViewModel, Common.Models.ApiContracts.Tenants.EditTenantContract>();
            CreateMap<ViewModels.Authentication.SignInViewModel, Common.Models.ApiContracts.Authentication.SignInRequestContract>();
            
            // ViewModel -> Logic model.
            CreateMap<TenantViewModel, Common.Models.Tenants.Tenant>();
            CreateMap<NewTenantViewModel, Common.Models.Tenants.Tenant>();
            CreateMap<EditTenantViewModel, Common.Models.Tenants.Tenant>();
            CreateMap<ViewModels.ExceptionProcessing.ErrorViewModel, WebAppStatusCodeException>();

            // Pagination.
            CreateMap<PaginationRequestViewModel, Pagination>().ConvertUsing(new PaginationRequestToModelTypeConverter());
            CreateMap<Pagination, PaginationResponseViewModel>().ConvertUsing<PaginationModelToResponseTypeConverter>();

            CreateMap(typeof(PagedResults<>), typeof(PagedResultsViewModel<>)).ConvertUsing(typeof(PagedResultsToViewModelTypeConverter<,>));

            CreateMap(typeof(Pagination), typeof(PagedResultsViewModel<>))
                .ForMember(nameof(PagedResultsViewModel<object>.Items), opt => opt.Ignore())
                .ForMember(nameof(PagedResultsViewModel<object>.Pagination), opt => opt.MapFrom(src => src));
        }
    }
}