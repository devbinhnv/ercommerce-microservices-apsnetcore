using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Entities;
using Ordering.Domain.Enums;

namespace Ordering.Infrastructure.Persistence.Configurations
{
    internal class OrderConfiguration : IEntityTypeConfiguration<OrderEntity>
    {
        public void Configure(EntityTypeBuilder<OrderEntity> builder)
        {
            builder.Property(e => e.Status)
                   .HasDefaultValue(EOrderStatus.New)
                   .IsRequired();
        }
    }
}
