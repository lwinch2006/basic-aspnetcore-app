using System.Collections;
using System.Collections.Generic;
using AutoMapper;
using Dka.AspNetCore.BasicWebApp.Common.Models.Pagination;
using Dka.AspNetCore.BasicWebApp.ViewModels.Pagination;

namespace Dka.AspNetCore.BasicWebApp.Services.AutoMapper
{
    public class PagedResultsToViewModelTypeConverter<TSource, TDestination> : ITypeConverter<PagedResults<TSource>, PagedResultsViewModel<TDestination>>
    {
        public PagedResultsViewModel<TDestination> Convert(PagedResults<TSource> source, PagedResultsViewModel<TDestination> destination, ResolutionContext context)
        {
            if (source == null)
            {
                return null;
            }
            
            destination ??= new PagedResultsViewModel<TDestination>();
            destination.Pagination ??= new PaginationResponseViewModel();
            destination.Items = context.Mapper.Map<IEnumerable<TDestination>>(source.Items);
            destination.Pagination.TotalCount = source.TotalCount;
            
            return destination;            
        }
    }
}