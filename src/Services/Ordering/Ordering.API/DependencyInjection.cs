using Carter;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using SharedKernel.Exceptions.Handler;

namespace Ordering.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        _ = services.AddCarter();

        _ = services.AddExceptionHandler<CustomExceptionHandler>();

        string connectionString = configuration.GetConnectionString("Database")
             ?? throw new InvalidOperationException("Database connection string is not configured.");

        _ = services.AddHealthChecks().AddSqlServer(connectionString);

        return services;
    }

    public static WebApplication UseApiServices(this WebApplication app)
    {
        _ = app.MapCarter();

        _ = app.UseExceptionHandler(static options => { });

        _ = app.UseHealthChecks("/health",
            new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

        return app;
    }
}
