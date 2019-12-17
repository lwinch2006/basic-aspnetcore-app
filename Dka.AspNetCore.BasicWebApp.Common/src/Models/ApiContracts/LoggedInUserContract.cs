using System;

namespace Dka.AspNetCore.BasicWebApp.Common.Models.ApiContracts
{
    public class LoggedInUserContract
    {
        public Guid Guid { get; set; }

        public string JwtToken { get; set; }
    }
}