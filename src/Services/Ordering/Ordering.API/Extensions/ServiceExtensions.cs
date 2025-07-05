using Infrastructure.Configurations;
using Shared.Configurations;
using MassTransit;
using Infrastructure.Extensions;
using Ordering.API.Application.IntegrationEvents.EventHandler;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Ordering.API.Extensions;

public static class ServiceExtensions
{
    internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
    {
        var smtpEmailSettings = configuration.GetSection(nameof(SMTPEmailSettings));
        services.Configure<SMTPEmailSettings>(smtpEmailSettings);

        return services;
    }

    internal static IServiceCollection ConfigureMassTransit(this IServiceCollection services)
    {
        var eventBusSettings = services.GetOptions<EventBusSettings>(EventBusSettings.Position);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(nameof(eventBusSettings.HostAddress));


        var mqConnection = new Uri(eventBusSettings.HostAddress);
        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
        services.AddMassTransit(config =>
        {
            config.AddConsumersFromNamespaceContaining<BasketCheckoutEventHandler>();
            config.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(mqConnection);
                // Add all of endpoints implement IConsumer interface
                cfg.ConfigureEndpoints(ctx);
            });
        });

        return services;
    }
}
