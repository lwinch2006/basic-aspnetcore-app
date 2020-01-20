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
                new ApplicationRole { Id = 1, Name = $"{nameof(UserRoleNames.Administrator)}", Guid = new Guid("7121e0ba-d1e2-4ffe-b6a6-836d4fcfd63b") },
                new ApplicationRole { Id = 2, Name = $"{nameof(UserRoleNames.Support)}", Guid = new Guid("781f674a-c097-4679-b7ef-40150d39fc32") },
                new ApplicationRole { Id = 3, Name = $"{nameof(UserRoleNames.PowerUser)}", Guid = new Guid("b1561d75-c8a4-41af-95f1-d81eb7f46b26") },
                new ApplicationRole { Id = 4, Name = $"{nameof(UserRoleNames.User)}", Guid = new Guid("3549e101-5dc2-48c5-b1a3-2fab66373983") },
            };

            return await Task.FromResult(dummyRoles);
        }         
    }
}