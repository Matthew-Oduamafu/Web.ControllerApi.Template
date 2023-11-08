using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Web.ControllerApi.Template.Data;
using Web.ControllerApi.Template.Options;
using Web.ControllerApi.Template.Repositories.Interfaces;
using Web.ControllerApi.Template.Repositories.Providers;
using Web.ControllerApi.Template.Services.Interfaces;
using Web.ControllerApi.Template.Services.Providers;

namespace Web.ControllerApi.Template.Extensions;

#pragma warning disable CS1591
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IPgRepository, PgRepository>();
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<ILinkService, LinkService>();
        return services;
    }

    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        // services.Configure<DatabaseConfig>()
        services.AddOptions<DatabaseConfig>()
            .BindConfiguration(nameof(DatabaseConfig))
            .Configure(c => { c.DbConnectionString = configuration.GetConnectionString("DefaultConnection")!; })
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            var databaseConfig = serviceProvider.GetRequiredService<IOptions<DatabaseConfig>>().Value;
            options.UseNpgsql(databaseConfig.DbConnectionString,
                optionsBuilder =>
                {
                    optionsBuilder.CommandTimeout(databaseConfig.CommandTimeout);
                    if (databaseConfig.EnableRetryOnFailure)
                        optionsBuilder.EnableRetryOnFailure(databaseConfig.MaxRetryCount,
                            TimeSpan.FromSeconds(databaseConfig.MaxRetryDelay),
                            databaseConfig.ErrorNumbersToAdd);

                    optionsBuilder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                });
        });
        return services;
    }

    public static IServiceCollection AddControllerConfiguration(this IServiceCollection services)
    {
        services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

        services.AddControllers(options =>
            {
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.WriteIndented = true;
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });
        return services;
    }

    public static void AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "API Title",
                Version = "v1",
                Description = "API description"
            });

            c.EnableAnnotations();
            
            // Optionally, add XML comments:
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });
    }

    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        });
    }
    // public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger logger)
    // {
    //     app.UseMiddleware<ExceptionMiddleware>(logger);
    // }
}