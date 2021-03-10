namespace Dka.AspNetCore.BasicWebApp.Common.Models.Constants
{
    public static class PaginationConstants
    {
        public static readonly string[] AvailablePageSizes = { Common.Hyphen, "10", "25", "50", "100", Common.All };
        
        public static class Common
        {
            public const string All = "All";
            public const string Hyphen = "-";
        }
    }
}