using Dka.AspNetCore.BasicWebApp.Common.Models.Pagination;

namespace Dka.AspNetCore.BasicWebApp.Common.Models.ApiContracts.Pagination
{
    public class PaginationRequestContract : IPaginationRequest
    {
        public int PageIndex { get; set; }
        public int? PageSize { get; set; }
    }
}