using ChatterBox.Application;
using ChatterBox.Services.Extensions;

try
{
    WebApplication
        .CreateBuilder(args)
        .ConfigureServices()
        .ConfigureAuthServiceAndBuild()
        .SetupCrucialSettings()
        .Run();
}
catch (Exception exc)
{
    Console.WriteLine($"Error has occurred and the program has been killed. Message: {exc.Message}");
}