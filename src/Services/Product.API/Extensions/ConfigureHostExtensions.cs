namespace Product.API.Extensions;

public static class ConfigureHostExtensions
{
    public static void AddAppConfigurations(this ConfigureHostBuilder host)
    {
        host.ConfigureAppConfiguration((ctx, cfg) =>
        {
            var env = ctx.HostingEnvironment;
            cfg.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
               .AddEnvironmentVariables();
        });
    }
    // Uncomment the following method if you want to use the WebApplicationBuilder instead of ConfigureHostBuilder
    //public static void AddAppConfiguration(this WebApplicationBuilder builder)
    //{
    //    var env = builder.Environment.EnvironmentName;
    //    builder.Configuration
    //           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    //           .AddJsonFile($"appsettings.{env}.json", optional: false, reloadOnChange: true)
    //           .AddEnvironmentVariables();
    //}
}