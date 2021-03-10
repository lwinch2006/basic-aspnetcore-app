using System.Collections.Generic;

namespace Dka.AspNetCore.BasicWebApp.ViewModels.Pagination
{
    public class PaginationSharedViewModel
    {
        public PaginationResponseViewModel Pagination { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public IDictionary<string, string> RouteValues { get; set; }        
    }
}