using Common.Logging;
using Serilog;
using Product.API.Extensions;
using Product.API.Persistence;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();

Log.Information("Starting Product API...");
try
{
    builder.Host.UseSerilog(SeriLogger.Configure);
    builder.Host.AddAppConfigurations();

    builder.Services.AddInfrastructure(builder.Configuration);

    var app = builder.Build();
    app.UseInfrastructure();
    app.MigrateDatabase<ProductContext>((context, _) =>
       {
           ProductContextSeed.SeedProductAsync(context, Log.Logger).Wait();
       })
       .Run();
}
catch (Exception ex)
{
    string exceptionType = ex.GetType().Name;
    if(exceptionType.Equals("StopTheHostException", StringComparison.Ordinal))
    {
        throw;
    }
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shutdown Product API complete");
    Log.CloseAndFlush();
}
