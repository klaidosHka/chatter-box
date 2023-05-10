using ChatterBox.Context;
using ChatterBox.Interfaces.Entities;
using ChatterBox.Services.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;

namespace ChatterBox.Application
{
    public static class WebAppExtensions
    {
        public static WebApplication UseConfiguredSettings(this WebApplication application)
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
                .UseCors("ChatterBoxCors")
                .UseHttpsRedirection()
                .UseStaticFiles()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization();

            application.MapRazorPages();

            application.UseEndpoints(ep =>
            {
                ep.MapHub<ChatHub>("/SignalR/Hub");
            });

            return application;
        }
    }
}
