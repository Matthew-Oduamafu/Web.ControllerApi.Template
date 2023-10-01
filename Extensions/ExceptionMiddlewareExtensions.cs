using Microsoft.EntityFrameworkCore;
using Web.ControllerApi.Template.CustomMiddlewares;
using Web.ControllerApi.Template.Data;

namespace Web.ControllerApi.Template.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app, ILogger logger)
    {
        return app.UseMiddleware<ExceptionMiddleware>(logger);
    }

    public static void UseSwaggerDocumentation(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
            c.RoutePrefix = "swagger";
        });
    }

    public static async Task MigrateDatabaseAsync(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices
            .GetRequiredService<IServiceScopeFactory>()
            .CreateScope();

        var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
        var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();

        try
        {
            if ((await context?.Database.GetPendingMigrationsAsync()!).Any())
            {
                logger.LogInformation("There are pending migrations. Applying now...");
                await context.Database.MigrateAsync();
                logger.LogInformation("All pending migrations applied successfully");
            }
            else
            {
                logger.LogInformation("No pending migrations found");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating the database");
            throw; // Re-throwing the exception to halt application startup.
        }
    }
}