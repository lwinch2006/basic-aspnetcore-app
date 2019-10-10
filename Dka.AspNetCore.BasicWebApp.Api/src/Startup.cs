using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Dka.AspNetCore.BasicWebApp.Api
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseRouting();
            
            app.UseEndpoints(configure =>
            {
                configure.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                configure.MapRazorPages();
            });
        }        
    }
}