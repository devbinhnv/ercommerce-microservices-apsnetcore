using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Infrastructure.Persistence;

namespace Ordering.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices (this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OrderContext>(opt =>
        {
            var builder = new SqlConnectionStringBuilder(configuration.GetConnectionString("DefaultConnectionString"));
            opt.UseSqlServer(builder.ConnectionString, 
                optionAction => optionAction.MigrationsAssembly(typeof(OrderContext).Assembly));
        });

        services.AddScoped<OderContextSeed>();
        return services;
    }

}
