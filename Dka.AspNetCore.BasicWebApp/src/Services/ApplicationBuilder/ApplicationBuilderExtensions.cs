using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;

namespace Dka.AspNetCore.BasicWebApp.Services.ApplicationBuilder
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder InsertLocalization(this IApplicationBuilder app, string[] supportedCultures)
        {
            app.UseRequestLocalization(options =>
            {
                options.SetDefaultCulture(supportedCultures[0]);
                options.AddSupportedCultures(supportedCultures);
                options.AddSupportedUICultures(supportedCultures);

                options.RequestCultureProviders = new List<IRequestCultureProvider>()
                {
                    new QueryStringRequestCultureProvider(),
                    new CookieRequestCultureProvider(),
                    new AcceptLanguageHeaderRequestCultureProvider()
                };
            });

            return app;
        }
    }
}