namespace Dka.AspNetCore.BasicWebApp.Common.Models.ApiContracts.Authentication
{
    public class SignInRequestContract
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }
}