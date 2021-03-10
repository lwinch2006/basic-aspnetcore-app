using System.Collections.Generic;
using System.Linq;
using Dka.AspNetCore.BasicWebApp.Common.Models.Pagination;

namespace Dka.AspNetCore.BasicWebApp.Common.Repositories.Extensions
{
    public static class Sql
    {
        public static string Where(IEnumerable<string> whereParts)
        {
            return !whereParts.Any() ? string.Empty : "WHERE " + string.Join(" AND ", whereParts);
        }
        
        public static string OrderWithPossiblePagination(Pagination pagination, string columnName)
        {
            return pagination?.PageSize > 0 && pagination.PageIndex >= 0
                ? $"ORDER BY {columnName} OFFSET @PageOffset ROWS FETCH NEXT @PageSize ROWS ONLY"
                : $"ORDER BY {columnName}";
        }
    }
}