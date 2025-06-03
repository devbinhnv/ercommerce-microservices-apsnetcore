using Common.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
// Use common config Seri logger
builder.Host.UseSerilog(SeriLogger.Configure);

Log.Information("Stating Product API");
try
{

    //builder.Host.UseSerilog((context, lc) =>
    //    lc.WriteTo.Console(outputTemplate: 
    //    "[{Timestamp:HH:mm:ss} {Level} {SourceContext}{NewLine}{Message:lgj}{NewLine}-{Exception}{NewLine}]").Enrich
    //              .FromLogContext().ReadFrom
    //              .Configuration(context.Configuration)
    //);

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
    Log.Information("Shutdown Product API complete");
    Log.CloseAndFlush();
}
