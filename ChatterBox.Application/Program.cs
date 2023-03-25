using ChatterBox.Application;

/*
 * Application is built within `WebAppExtensions.cs`.
 * 
 * Following two chained methods before the #Run
 * method call execute the main start-up logic.
 */

WebApplication
    .CreateBuilder(args)
    .BuildAppWithDatabaseAndGoogleAuth()
    .SetupApplicationRoutingAndAuth()
    .Run();