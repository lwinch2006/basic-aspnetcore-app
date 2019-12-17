using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Dka.AspNetCore.BasicWebApp.Common.Models.Configurations
{
    public class JwtConfiguration
    {
        public string Secret { get; set; }

        public JwtBearerOptions JwtBearerOptions { get; set; }
    }
}