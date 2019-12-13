using System;
using Microsoft.AspNetCore.Identity;

namespace Dka.AspNetCore.BasicWebApp.Common.Models.Authentication
{
    public class ApplicationUser : IdentityUser<int>
    {
        public Guid Guid { get; set; }

    }
}