using System.ComponentModel.DataAnnotations;

namespace Dka.AspNetCore.BasicWebApp.Areas.Administration.ViewModels.Tenants
{
    public class EditTenantViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "Length should be less or equal 100")]        
        public string Name { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Length should be less or equal 50")]        
        public string Alias { get; set; }          
    }
}