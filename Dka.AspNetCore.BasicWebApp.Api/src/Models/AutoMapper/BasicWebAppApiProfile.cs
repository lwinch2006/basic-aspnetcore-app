using System;
using AutoMapper;
using Dka.AspNetCore.BasicWebApp.Api.Services.AutoMapper;
using Dka.AspNetCore.BasicWebApp.Common.Logic.AutoMapper;
using Dka.AspNetCore.BasicWebApp.Common.Models.ApiContracts.Pagination;
using Dka.AspNetCore.BasicWebApp.Common.Models.Pagination;

namespace Dka.AspNetCore.BasicWebApp.Api.Models.AutoMapper
{
    public class BasicWebAppApiProfile : Profile
    {
        public BasicWebAppApiProfile()
        {
            // Logic model -> API contract.
            CreateMap<Common.Models.Tenants.Tenant, Common.Models.ApiContracts.Tenants.TenantContract>();
            CreateMap<Common.Models.Tenants.Tenant, Common.Models.ApiContracts.Tenants.NewTenantContract>();
            CreateMap<Common.Models.Tenants.Tenant, Common.Models.ApiContracts.Tenants.EditTenantContract>();
            
            
            // API contract -> Logic model.
            CreateMap<Common.Models.ApiContracts.Tenants.TenantContract, Common.Models.Tenants.Tenant>();
            CreateMap<Common.Models.ApiContracts.Tenants.EditTenantContract, Common.Models.Tenants.Tenant>();
            CreateMap<Common.Models.ApiContracts.Tenants.NewTenantContract, Common.Models.Tenants.Tenant>()
                .ForMember(dst => dst.CreatedOnUtc, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dst => dst.CreatedBy, opt => opt.MapFrom<UserIdResolver>());

            // Pagination.
            CreateMap<PaginationRequestContract, Pagination>().ConvertUsing(new PaginationRequestToModelTypeConverter());
            CreateMap(typeof(PagedResults<>), typeof(PagedResults<>));
        }
    }
}