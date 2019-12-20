using Microsoft.AspNetCore.Identity;

namespace Dka.AspNetCore.BasicWebApp.Common.Models.ApiContracts.Authentication
{
    public class SignOutResponseContract
    {
        public SignInResult SignOutResult { get; set; }
    }
}