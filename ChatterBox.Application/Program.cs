using ChatterBox.Application;
using ChatterBox.Services.Extensions;

try
{
    WebApplication
        .CreateBuilder(args)
        .ConfigureServices()
        .ConfigureAuthServiceAndBuild()
        .UseConfiguredSettings()
        .Run();
}
catch (Exception e) when (e is not OperationCanceledException && e.GetType().Name != "StopTheHostException")
{
    Console.WriteLine($"Error has occurred and the program has been killed. Message: {e.Message}");

    Console.ReadKey();
}