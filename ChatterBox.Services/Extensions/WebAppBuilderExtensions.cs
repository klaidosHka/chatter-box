using ChatterBox.Context;
using ChatterBox.Interfaces.Entities;
using ChatterBox.Interfaces.Repositories;
using ChatterBox.Interfaces.Services;
using ChatterBox.Services.Repositories;
using ChatterBox.Services.Services;
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
            },
                contextLifetime: ServiceLifetime.Singleton,
                optionsLifetime: ServiceLifetime.Singleton
            );

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services
                .AddRazorPages()
                .AddRazorPagesOptions(o =>
                {
                    o.Conventions.AuthorizeAreaPage("Main", "/Index");
                    o.Conventions.AddAreaPageRoute("Identity", "/Account/Login", string.Empty);
                });

            builder.Services
                .AddDefaultIdentity<ChatUser>(o =>
                {
                    o.SignIn.RequireConfirmedAccount = false;
                    o.Password.RequireDigit = false;
                    o.Password.RequireLowercase = false;
                    o.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<CbDbContext>();

            // Services
            builder.Services.AddSingleton<IChatGroupMessageService, ChatGroupMessageService>();
            builder.Services.AddSingleton<IChatGroupRegistrarService, ChatGroupRegistrarService>();
            builder.Services.AddSingleton<IChatGroupService, ChatGroupService>();
            builder.Services.AddSingleton<IChatMessageService, ChatMessageService>();
            builder.Services.AddSingleton<IChatUserService, ChatUserService>();

            // Repositories
            builder.Services.AddSingleton<IChatGroupMessageRepository, ChatGroupMessageRepository>();
            builder.Services.AddSingleton<IChatGroupRegistrarRepository, ChatGroupRegistrarRepository>();
            builder.Services.AddSingleton<IChatGroupRepository, ChatGroupRepository>();
            builder.Services.AddSingleton<IChatMessageRepository, ChatMessageRepository>();
            builder.Services.AddSingleton<IChatUserRepository, ChatUserRepository>();

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
