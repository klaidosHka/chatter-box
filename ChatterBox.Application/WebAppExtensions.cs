using ChatterBox.Context;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ChatterBox.Application
{
    public static class WebAppExtensions
    {
        public static WebApplication BuildAppWithDatabaseAndGoogleAuth(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<CbDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseAzure"));
            });

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services
                .AddDefaultIdentity<IdentityUser>(o =>
                {
                    o.SignIn.RequireConfirmedAccount = false;
                    o.Password.RequireDigit = false;
                    o.Password.RequireLowercase = false;
                    o.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<CbDbContext>();

            builder.Services
                .AddRazorPages()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AddAreaPageRoute("Identity", "/Account/Login", "");
                });

            builder.Services
                .AddAuthentication()
                .AddCookie(o =>
                {
                    o.AccessDeniedPath = "/Identity/Account/AccessDenied";

                    o.Cookie = new CookieBuilder
                    {
                        SameSite = SameSiteMode.Strict,
                        SecurePolicy = CookieSecurePolicy.Always,
                        IsEssential = true,
                        HttpOnly = true,
                        Name = "ChatterBoxCookie"
                    };

                    o.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                    o.LoginPath = "/Identity/Account/Login";
                    o.SlidingExpiration = true;

                })
                .AddGoogle(o =>
                {
                    o.ClientId = builder.Configuration["Auth:Google:ClientId"];
                    o.ClientSecret = builder.Configuration["Auth:Google:ClientSecret"];
                    o.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
                });

            return builder.Build();
        }

        public static WebApplication SetupApplicationRoutingAndAuth(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app
                .UseHttpsRedirection()
                .UseStaticFiles()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization();

            app.MapRazorPages();

            return app;
        }
    }
}
