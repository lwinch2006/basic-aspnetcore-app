using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Dka.AspNetCore.BasicWebApp.ViewModels.Pagination
{
    public class PaginationResponseViewModel
    {
        public int PageIndex { get; set; }
        public int? PageSize { get; set; }
        public int TotalCount { get; set; }
        public string PageSizeAsString { get; set; }
        public IEnumerable<SelectListItem> PageSizes { get; set; }        
    }
}