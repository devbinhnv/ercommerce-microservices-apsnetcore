using Common.Logging;
using Contracts.Domains.Interfaces;
using Contracts.Services;
using Infrastructure.Common;
using Infrastructure.Services;
using Ordering.API.Extensions;
using Ordering.Application;
using Ordering.Application.Common.Interfaces;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistence;
using Ordering.Infrastructure.Repositories;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();

Log.Information($"Starting {builder.Environment.ApplicationName}");
try
{
    // Use common config Seri logger
    builder.Host.UseSerilog(SeriLogger.Configure);

    builder.Host.AddAppConfigurations();
    // Add services to the container.

    builder.Services.AddConfigurationSettings(builder.Configuration);
    builder.Services.AddInfrastructureServices(builder.Configuration);
    builder.Services.AddApplicationServices();
    builder.Services.ConfigureMassTransit();

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddScoped<IOrderRepository, OrderRepository>();
    builder.Services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));

    builder.Services.AddScoped<ISmtpEmailService, SmtpEmailService>();

    var app = builder.Build();

    using (var scope = app.Services.CreateScope())
    {
        var orderContextSeed = scope.ServiceProvider.GetRequiredService<OderContextSeed>();
        await orderContextSeed.InitializeAsync();
        await orderContextSeed.SeedAsync();
    }

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    //app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    string exceptionType = ex.GetType().Name;
    if (exceptionType.Equals("StopTheHostException", StringComparison.Ordinal))
    {
        throw;
    }
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shutdown Ordering API complete");
    Log.CloseAndFlush();
}
