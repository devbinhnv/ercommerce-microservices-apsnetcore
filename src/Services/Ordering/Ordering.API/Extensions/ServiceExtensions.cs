using Infrastructure.Configurations;

namespace Ordering.API.Extensions;

public static class ServiceExtensions
{
    internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
    {
        var smtpEmailSettings = configuration.GetSection(nameof(SMTPEmailSettings));
        services.Configure<SMTPEmailSettings>(smtpEmailSettings);

        return services;
    }
}
