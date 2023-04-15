using ChatterBox.Context;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChatterBox.Services.Extensions
{
    public static class WebAppBuilderExtensions
    {
        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
        {
            // Database & Auth
            builder.Services.AddDbContext<CbDbContext>(o =>
            {
                o.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseAzure"));
            });

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services
                .AddRazorPages()
                .AddRazorPagesOptions(o =>
                {
                    o.Conventions.AuthorizeAreaPage("Main", "/Index");
                    o.Conventions.AddAreaPageRoute("Identity", "/Account/Login", string.Empty);
                });

            builder.Services
                .AddDefaultIdentity<IdentityUser>(o =>
                {
                    o.SignIn.RequireConfirmedAccount = false;
                    o.Password.RequireDigit = false;
                    o.Password.RequireLowercase = false;
                    o.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<CbDbContext>();

            // Services

            // Repositories

            return builder;
        }

        public static WebApplication ConfigureAuthServiceAndBuild(this WebApplicationBuilder builder)
        {
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
    }
}
