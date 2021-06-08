using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dka.AspNetCore.BasicWebApp.Common.Models.Toastr
{
    public enum ToastrMessageTypes
    {
        [Display(Name = "Information")]
        [Description("Information")]
        Info = 1,

        [Display(Name = "Success")]
        [Description("Success")]
        Success  = 2,
        
        [Display(Name = "Warning")]
        [Description("Warning")]
        Warning  = 3,
        
        [Display(Name = "Error")]
        [Description("Error")]
        Error = 4
    }
}