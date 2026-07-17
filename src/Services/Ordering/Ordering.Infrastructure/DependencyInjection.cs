using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices
        (this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("Database");

        //_ = services.AddDbContext<OrderingDbContext>(options =>
        //    options.UseSqlServer(connectionString,
        //        sqlOptions => sqlOptions.EnableRetryOnFailure()));

        //_ = services.AddScoped<IOrderRepository, OrderRepository>();

        return services;
    }
}
