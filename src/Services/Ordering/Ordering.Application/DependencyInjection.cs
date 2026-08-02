using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using SharedKernel.Behaviors;
using FluentValidation;
using SharedKernel.Messaging.MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;

namespace Ordering.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddMediatR(static cfg =>
            {
                _ = cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                _ = cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
                _ = cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            });

        _ = services.AddFeatureManagement();

        _ = services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        _ = services.AddMessageBroker(configuration, Assembly.GetExecutingAssembly());

        return services;
    }
}
