using Microsoft.Extensions.Hosting;
using Serilog;


namespace Common.Logging
{
    public static class SeriLogger
    {
        public static Action<HostBuilderContext, LoggerConfiguration> Configure =>
            (context, configuration) =>
            {
                var applicationName = context.HostingEnvironment.ApplicationName?.ToLower().Replace(".", "-");
                var environmentName = context.HostingEnvironment.EnvironmentName ?? "Development";
                configuration
                    .WriteTo.Debug()
                    .WriteTo.Console(outputTemplate:
                        "[{Timestamp:HH:mm:ss} {Level} {SourceContext}{NewLine}{Message:lgj}{NewLine}-{Exception}{NewLine}]")
                    .Enrich.FromLogContext()
                    //Need: install package Serilog.Enrichers;
                    .Enrich.WithMachineName()
                    .Enrich.WithProperty("ApplicationName", applicationName)
                    .Enrich.WithProperty("EnvironmentName", environmentName)
                    .ReadFrom.Configuration(context.Configuration);
            };
    }
}
