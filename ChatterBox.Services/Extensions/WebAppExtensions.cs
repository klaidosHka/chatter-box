using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace ChatterBox.Application
{
    public static class WebAppExtensions
    {
        public static WebApplication SetupCrucialSettings(this WebApplication application)
        {
            if (application.Environment.IsDevelopment())
            {
                application.UseMigrationsEndPoint();
            }
            else
            {
                application.UseExceptionHandler("/Error");
                application.UseHsts();
            }

            application
                .UseHttpsRedirection()
                .UseStaticFiles()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization();

            application.MapRazorPages();

            return application;
        }
    }
}
