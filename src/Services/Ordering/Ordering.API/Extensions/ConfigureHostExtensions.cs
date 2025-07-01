namespace Ordering.API.Extensions;

internal static class ConfigureHostExtensions
{
    internal static void AddAppConfigurations(this ConfigureHostBuilder host)
    {
        host.ConfigureAppConfiguration((ctx, cfg) =>
        {
            var env = ctx.HostingEnvironment;
            cfg.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
               .AddEnvironmentVariables();
        });
    }
}
