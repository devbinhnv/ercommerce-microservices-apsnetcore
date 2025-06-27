using Serilog;

namespace Basket.API.Extensions;

public static class HostExtentions
{
    public static void AddAppConfiguration(this WebApplicationBuilder builder)
    {
        var env = builder.Environment.EnvironmentName;
        builder.Configuration
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .AddJsonFile($"appsettings.{env}.json", optional: false, reloadOnChange: true)
               .AddEnvironmentVariables();
    }
}
