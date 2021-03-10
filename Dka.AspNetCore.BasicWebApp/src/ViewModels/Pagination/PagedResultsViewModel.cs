using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Dka.AspNetCore.BasicWebApp.ViewModels.Pagination
{
    public class PagedResultsViewModel<T> 
    {
        public IEnumerable<T> Items { get; set; }
        public PaginationResponseViewModel Pagination { get; set; }

        public static PagedResultsViewModel<T> InitEmpty()
        {
            var result = new PagedResultsViewModel<T>
            {
                Items = Enumerable.Empty<T>(),
                Pagination = new PaginationResponseViewModel
                {
                    PageIndex = 0,
                    PageSize = 0,
                    PageSizeAsString = "0",
                    PageSizes = Enumerable.Empty<SelectListItem>(),
                    TotalCount = 0
                }
            };

            return result;
        }
    }
}