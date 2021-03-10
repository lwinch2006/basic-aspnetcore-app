using System.Collections.Generic;

namespace Dka.AspNetCore.BasicWebApp.Common.Models.Pagination
{
    public class PagedResults<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalCount { get; set; }
    }
}