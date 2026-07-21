using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using SharedKernel.Behaviors;

namespace Ordering.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
    {
        _ = services.AddMediatR(static cfg =>
            {
                _ = cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                _ = cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
                _ = cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            });

        return services;
    }
}
