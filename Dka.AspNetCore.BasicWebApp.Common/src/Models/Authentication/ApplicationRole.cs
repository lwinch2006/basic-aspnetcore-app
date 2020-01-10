using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Common.Models.Constants;
using Microsoft.AspNetCore.Identity;

namespace Dka.AspNetCore.BasicWebApp.Common.Models.Authentication
{
    public class ApplicationRole : IdentityRole<int>
    {
        public Guid Guid { get; set; }
        
        public static async Task<IEnumerable<ApplicationRole>> GetDummyRoleSet()
        {
            var dummyRoles = new List<ApplicationRole>
            {
                new ApplicationRole { Id = 1, Name = $"{nameof(UserRoleNames.Administrator).ToLower()}", Guid = new Guid("7121e0ba-d1e2-4ffe-b6a6-836d4fcfd63b") },
                new ApplicationRole { Id = 2, Name = $"{nameof(UserRoleNames.Support).ToLower()}", Guid = new Guid("781f674a-c097-4679-b7ef-40150d39fc32") }
            };

            return await Task.FromResult(dummyRoles);
        }         
    }
}