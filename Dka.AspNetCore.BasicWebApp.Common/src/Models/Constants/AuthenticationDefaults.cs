namespace Dka.AspNetCore.BasicWebApp.Common.Models.Constants
{
    public static class AuthenticationDefaults
    {
        public const string LoginUrl = "/Account/Login/SignIn";
        
        public const string LogoutUrl = "/Account/Login/SignOut";
        
        public const string AccessDeniedUrl = "/Account/AccessDenied";

        public const string ReturnUrlParameter = "returnUrl";
    }
}