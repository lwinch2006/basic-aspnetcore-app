using System;
using Microsoft.AspNetCore.Identity;

namespace Dka.AspNetCore.BasicWebApp.Common.Models.ApiContracts.Authentication
{
    public class SignInResponseContract
    {
        public DateTime ExpireAt { get; set; }

        public string AccessToken { get; set; }

        public string UserRole { get; set; }
    }
}