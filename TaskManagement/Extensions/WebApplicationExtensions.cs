using Domain.Contracts;
using TaskManagement.MiddleWares;

namespace TaskManagement.Extensions
{
    public static class WebApplicationExtensions
    {
        public static async Task<WebApplication> SeedDbAsync(this WebApplication app)
        {


            using var scope = app.Services.CreateScope();



            var DBInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();


            await DBInitializer.InitializeIdentityAsync();

            return app;
        }
        public static WebApplication UseCustomMiddleware(this WebApplication app)
        { //--> Used if we add more Middlewares


            app.UseMiddleware<GlobalErrorHandlingMiddleware>();


            // This will call InvokeAsync() in GlobalExceptionMiddleware
            //ASP.NET Core framework itself calls the InvokeAsync() method for every middleware in the pipeline.

            return app;
        }
    }
}


//using var scope = app.Services.CreateScope();:

//This creates a scope (temporary container) for services in the dependency injection (DI) system.
//A scope ensures that services are resolved correctly during the application’s startup, without leaking memory.

//This scope is needed to get services like IDbInitializer that are registered in the DI container.


//var DbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
//scope.ServiceProvider.GetRequiredService<IDbInitializer>():
//This line retrieves the IDbInitializer service from the DI container.

//IDbInitializer is typically a service that contains the logic for seeding data (creating roles, users, etc.).

//GetRequiredService ensures that the service is found in the container, or it throws an error if it’s not available.