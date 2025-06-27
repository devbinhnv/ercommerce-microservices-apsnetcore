using Basket.API.Extensions;
using Common.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();

Log.Information($"Stating Inventory {builder.Environment.ApplicationName}");
try
{
    builder.AddAppConfiguration();
    builder.Host.UseSerilog(SeriLogger.Configure);

    // Add services to the container.
    builder.Services.AddInfratuctures(builder.Configuration);
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{builder.Environment.ApplicationName} v1");
        });
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception err)
{
    Log.Fatal(err, "Unhandled exception");
}
finally
{
    Log.Information("Shutdown Basket API complete");
    Log.CloseAndFlush();
}
