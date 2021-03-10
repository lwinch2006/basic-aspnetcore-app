using Dka.AspNetCore.BasicWebApp.Common.Models.Pagination;

namespace Dka.AspNetCore.BasicWebApp.ViewModels.Pagination
{
    public class PaginationRequestViewModel : IPaginationRequest
    {
        public int PageIndex { get; set; }
        public int? PageSize { get; set; }
    }
}