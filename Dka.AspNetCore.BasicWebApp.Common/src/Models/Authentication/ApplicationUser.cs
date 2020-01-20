using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Common.Models.Constants;
using Microsoft.AspNetCore.Identity;

namespace Dka.AspNetCore.BasicWebApp.Common.Models.Authentication
{
    public class ApplicationUser : IdentityUser<int>
    {
        public Guid Guid { get; set; }

        public static async Task<IEnumerable<ApplicationUser>> GetDummyUserSet()
        {
            var dummyUsers = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = 1,
                    UserName = $"{nameof(UserRoleNames.Administrator).ToLower()}@basicwebapp.com",
                    Email = $"{nameof(UserRoleNames.Administrator).ToLower()}@basicwebapp.com",
                    EmailConfirmed = true,
                    Guid = new Guid("5b66df8d-0cb3-4af9-8df1-c0013544da23")
                },
                new ApplicationUser
                {
                    Id = 2,
                    UserName = $"{nameof(UserRoleNames.Support).ToLower()}@basicwebapp.com",
                    Email = $"{nameof(UserRoleNames.Support).ToLower()}@basicwebapp.com",
                    EmailConfirmed = true,
                    Guid = new Guid("21bbe777-1f4a-43ba-9271-2bb038c868df")
                },
                new ApplicationUser
                {
                    Id = 2,
                    UserName = $"{nameof(UserRoleNames.PowerUser).ToLower()}@basicwebapp.com",
                    Email = $"{nameof(UserRoleNames.PowerUser).ToLower()}@basicwebapp.com",
                    EmailConfirmed = true,
                    Guid = new Guid("2f4a697d-d12c-4bb2-9116-0107f2de424c")
                },
                new ApplicationUser
                {
                    Id = 2,
                    UserName = $"{nameof(UserRoleNames.User).ToLower()}@basicwebapp.com",
                    Email = $"{nameof(UserRoleNames.User).ToLower()}@basicwebapp.com",
                    EmailConfirmed = true,
                    Guid = new Guid("cc377435-39b2-4644-afde-8d06f7fa0824")
                }
            };

            return await Task.FromResult(dummyUsers);
        }
    }
}