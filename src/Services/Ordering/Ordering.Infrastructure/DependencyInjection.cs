using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Data;

namespace Ordering.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices
        (this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("Database")
            ?? throw new InvalidOperationException("Connection string 'Database' was not found.");

        // Add services to the container.
        _ = services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        _ = services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        _ = services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseSqlServer(connectionString);
        });

        _ = services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

        return services;
    }
}
