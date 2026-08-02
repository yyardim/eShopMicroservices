using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace SharedKernel.Messaging.MassTransit;

public static class Extensions
{
    public static IServiceCollection AddMessageBroker
        (this IServiceCollection services, IConfiguration configuration, Assembly? assembly = null)
    {
        _ = services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();

            if (assembly != null)
                config.AddConsumers(assembly);

            string hostAddress = configuration["MessageBroker:Host"]
                ?? throw new InvalidOperationException("MessageBroker:Host is required.");
            string userName = configuration["MessageBroker:UserName"]
                ?? throw new InvalidOperationException("MessageBroker:UserName is required.");
            string password = configuration["MessageBroker:Password"]
                ?? throw new InvalidOperationException("MessageBroker:Password is required.");

            config.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(hostAddress), host =>
                {
                    host.Username(userName);
                    host.Password(password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
