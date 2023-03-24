using ChatterBox.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ------------------------------------------------------------------------------
// Add services to the container.

builder.Services.AddDbContext<CbDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseAWS"));
});

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services
    .AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<CbDbContext>();

builder.Services.AddRazorPages();

builder.Services
    .AddAuthentication()
    .AddGoogle(o =>
    {
        o.ClientId = builder.Configuration["Auth:Google:ClientId"];
        o.ClientSecret = builder.Configuration["Auth:Google:ClientSecret"];
    });

var app = builder.Build();

// ------------------------------------------------------------------------------
// Setup app.

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

// ------------------------------------------------------------------------------

app.Run();
