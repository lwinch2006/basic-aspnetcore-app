using System.Linq;
using AutoMapper;
using Dka.AspNetCore.BasicWebApp.Common.Models.Constants;
using Dka.AspNetCore.BasicWebApp.Common.Models.Pagination;

namespace Dka.AspNetCore.BasicWebApp.Common.Logic.AutoMapper
{
    public class PaginationRequestToModelTypeConverter : ITypeConverter<IPaginationRequest, Pagination>
    {
        public Pagination Convert(IPaginationRequest source, Pagination destination, ResolutionContext context)
        {
            if (source == null)
            {
                return null;
            }

            var pageSize = source.PageSize.HasValue
                ? source.PageSize < 0 ? 0 : source.PageSize.Value
                : int.Parse(PaginationConstants.AvailablePageSizes.Skip(1).First());

            destination ??= new Pagination();
            destination.PageIndex = source.PageIndex < 0 ? 0 : source.PageIndex;
            destination.PageSize = pageSize;
        
            return destination;
        }
    }
}