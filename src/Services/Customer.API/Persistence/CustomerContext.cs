using Customer.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Customer.API.Persistence;

public class CustomerContext : DbContext
{
    public CustomerContext(DbContextOptions<CustomerContext> options) : base(options)
    {
        
    }
    
    public DbSet<CustomerEntity> Customers { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<CustomerEntity>(entity =>
        {
            entity.HasIndex(e => e.Id).IsUnique();
            entity.HasIndex(e => e.EmailAddress).IsUnique();
        });
    }
}
