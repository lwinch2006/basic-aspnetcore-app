using System;
using System.ComponentModel.DataAnnotations;
using Dka.AspNetCore.BasicWebApp.Common.Logic.Validators;

namespace Dka.AspNetCore.BasicWebApp.Common.Models.ApiContracts.Tenants
{
    public class TenantContract
    {
        [Required]
        [NotEmptyGuid]
        public Guid Guid { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Length should be less or equal 100")]        
        public string Name { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Length should be less or equal 50")]
        public string Alias { get; set; }        
    }
}