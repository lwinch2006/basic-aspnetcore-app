using System.ComponentModel.DataAnnotations;
using Dka.AspNetCore.BasicWebApp.Common.Logic.Utils;
using Newtonsoft.Json;

namespace Dka.AspNetCore.BasicWebApp.ViewModels.Authentication
{
    [JsonConverter(typeof(JsonPathConverter))]
    public class SignInViewModel
    {
        [JsonProperty(PropertyName = "username")]
        [Required]
        public string Username { get; set; }

        [JsonProperty(PropertyName = "path.to.password")]
        [Required]
        public string Password { get; set; }
    }
}