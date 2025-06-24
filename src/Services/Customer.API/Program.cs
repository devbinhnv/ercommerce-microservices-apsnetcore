using Common.Logging;
using Contracts.Common;
using Customer.API.Persistence;
using Customer.API.Repositories;
using Customer.API.Repositories.Interfaces;
using Customer.API.Services;
using Customer.API.Services.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Serilog;
using CustomerEntity = Customer.API.Entities.Customer;

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

    builder.Services.AddDbContext<CustomerContext>(opt =>
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");
        opt.UseNpgsql(connectionString);
    });
    builder.Services.AddScoped<ICustomerRepository, CustomerRepository>()
        .AddScoped(typeof(IRepositoryBaseAsync<,,>), typeof(RepositoryBaseAsync<,,>))
        .AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>))
        .AddScoped<ICustomerService, CustomerService>();

    var app = builder.Build();
    app.MapGet("/", () => "Welcome to customer API");
    app.MapGet("/api/customers", async (ICustomerService customerService) 
        => await customerService.GetCustomersAsync());

    app.MapGet("/api/customers/{customerName}", 
        async (string customerName, ICustomerService customerService) => await customerService.GetCustomerByUserNameAsync(customerName));

    app.MapPost("/api/customers",
        async (CustomerEntity newCustomer, ICustomerRepository customerRepository) =>
        {
           await customerRepository.CreateAsync(newCustomer);
           await customerRepository.SaveChangeAsync();
           return Results.NoContent();
        });

    app.MapDelete("/api/customers/{id:int}", async (int id, ICustomerRepository customerRepository) =>
    {
        var customer = await customerRepository
            .FindByCondition(x => x.Id.Equals(id))
            .SingleOrDefaultAsync();
        if (customer == null) return Results.NotFound();

        await customerRepository.DeleteAsync(customer);
        await customerRepository.SaveChangeAsync();
        return Results.NoContent();
    });

    //app.MapPut("/api/customers/{id}",() => "");

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    //app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.SeedCustomerData()
        .Run();
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
    Log.Information("Shutdown Customer API complete");
    Log.CloseAndFlush();
}
