namespace Dka.AspNetCore.BasicWebApp.Common.Models.Pagination
{
    public interface IPaginationRequest
    {
        public int PageIndex { get; set; }
        public int? PageSize { get; set; }          
    }
}