using Microsoft.EntityFrameworkCore;

namespace Product.API.Extensions;

public static class HostExtensions
{
    public static IHost MigrateDatabase<TContext> (this IHost host, Action<TContext, IServiceProvider> seeder)
        where TContext : DbContext
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetService<ILogger<TContext>>();
        var context = services.GetService<TContext>();

        try
        {
            logger?.LogInformation("Migrating mysql database.");
            ExecuteMigrations(context);
            logger?.LogInformation("Migrated mysql database.");
            InvokeSeader(seeder, context, services);
        }
        catch (Exception ex)
        {
            logger?.LogError(ex, "An error occurred while migrating the mysql database.");
            throw;
        }
        return host;
    }

    private static void InvokeSeader<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider services) 
        where TContext : DbContext
    {
        seeder(context, services);
    }

    private static void ExecuteMigrations<TContext>(TContext? context)
        where TContext : DbContext
    {
        context?.Database.Migrate();
    }
}
