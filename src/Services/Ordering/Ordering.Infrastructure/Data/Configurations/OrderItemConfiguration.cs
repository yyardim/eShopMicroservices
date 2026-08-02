using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        _ = builder.HasKey(static oi => oi.Id);
        _ = builder.Property(static oi => oi.Id)
            .HasConversion(
                static orderItemId => orderItemId.Value,
                static dbId => OrderItemId.From(dbId));

        _ = builder.HasOne<Product>()
            .WithMany()
            .HasForeignKey(static oi => oi.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        _ = builder.Property(static oi => oi.Quantity).IsRequired();

        _ = builder.Property(static oi => oi.Price).HasColumnType("decimal(18,2)").IsRequired();
    }
}
