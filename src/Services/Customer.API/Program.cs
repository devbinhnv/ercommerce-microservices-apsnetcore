using Common.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
// Use common config Seri logger
builder.Host.UseSerilog(SeriLogger.Configure);

Log.Information("Stating Customer API");
try
{
    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
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
    Log.Information("Shutdown Customer API complete");
    Log.CloseAndFlush();
}
