using ChatterBox.Context;
using Microsoft.AspNetCore.Builder;
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
                .UseCors(pb =>
                {
                    pb
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithOrigins("https://localhost:44340");
                })
                .UseWebSockets()
                .UseHttpsRedirection()
                .UseStaticFiles()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization();

            application.MapRazorPages();

            application.UseEndpoints(ep =>
            {
                ep.MapHub<ChatHub>("/Main/Index");
            });

            return application;
        }
    }
}
