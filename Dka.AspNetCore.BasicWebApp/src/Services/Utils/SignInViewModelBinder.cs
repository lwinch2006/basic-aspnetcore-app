using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.ViewModels.Authentication;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace Dka.AspNetCore.BasicWebApp.Services.Utils
{
    public class SignInViewModelBinder : IModelBinder
    {
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            string json;

            using (var sr = new StreamReader(bindingContext.HttpContext.Request.Body, Encoding.UTF8))
            {
                json = await sr.ReadToEndAsync();
                sr.Close();
            }

            if (string.IsNullOrWhiteSpace(json))
            {
                return;
            }

            var model = JsonConvert.DeserializeObject<SignInViewModel>(json);
            bindingContext.Result = ModelBindingResult.Success(model);
        }         
    }
}