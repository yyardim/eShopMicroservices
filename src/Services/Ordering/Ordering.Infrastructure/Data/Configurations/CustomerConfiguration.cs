using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        _ = builder.HasKey(static c => c.Id);
        _ = builder.Property(static c => c.Id)
            .HasConversion(
                static customerId => customerId.Value,
                static dbId => CustomerId.Of(dbId));

        _ = builder.Property(static c => c.Name).HasMaxLength(100).IsRequired();

        _ = builder.Property(static c => c.Email).HasMaxLength(255);
        _ = builder.HasIndex(static c => c.Email).IsUnique();
    }
}
