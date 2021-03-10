using System.Linq;
using AutoMapper;
using Dka.AspNetCore.BasicWebApp.Common.Models.Constants;
using Dka.AspNetCore.BasicWebApp.ViewModels.Pagination;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Dka.AspNetCore.BasicWebApp.Services.AutoMapper
{
    public class PaginationModelToResponseTypeConverter : ITypeConverter<Common.Models.Pagination.Pagination, PaginationResponseViewModel>
    {
        public PaginationResponseViewModel Convert(Common.Models.Pagination.Pagination source, PaginationResponseViewModel destination,
            ResolutionContext context)
        {
            if (source == null)
            {
                return null;
            }

            var pageSizeAsString =
                PaginationConstants.AvailablePageSizes.SingleOrDefault(t => t.Equals(source.PageSize.ToString())) ??
                (source.PageSize > 0 ? PaginationConstants.Common.Hyphen : PaginationConstants.Common.All);
            
            destination ??= new PaginationResponseViewModel();
            destination.PageIndex = source.PageIndex;
            destination.PageSize = source.PageSize;
            destination.PageSizeAsString = pageSizeAsString;
            destination.PageSizes = PaginationConstants.AvailablePageSizes
                .Skip(pageSizeAsString.Equals(PaginationConstants.Common.Hyphen) ? 0 : 1)
                .Select(t => new SelectListItem {Value = t, Text = t});
            
            return destination;            
        }
    }
}