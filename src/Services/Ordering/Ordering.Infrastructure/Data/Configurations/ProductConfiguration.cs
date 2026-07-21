using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        _ = builder.HasKey(static p => p.Id);
        _ = builder.Property(static p => p.Id)
            .HasConversion(
                static productId => productId.Value,
                static dbId => ProductId.Of(dbId));

        _ = builder.Property(static p => p.Name).HasMaxLength(100).IsRequired();
    }
}
