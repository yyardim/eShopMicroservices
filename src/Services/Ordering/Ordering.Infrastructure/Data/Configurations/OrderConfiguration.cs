using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Enums;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        _ = builder.Ignore(static o => o.DomainEvents);

        _ = builder.HasKey(static o => o.Id);
        _ = builder.Property(static o => o.Id)
            .HasConversion(
                static orderId => orderId.Value,
                static dbId => OrderId.Of(dbId));

        _ = builder.HasOne<Customer>()
            .WithMany()
            .HasForeignKey(static o => o.CustomerId)
            .IsRequired();

        _ = builder.HasMany(o => o.OrderItems)
            .WithOne()
            .HasForeignKey(static oi => oi.OrderId);

        _ = builder.ComplexProperty(
            o => o.OrderName, static nameBuilder =>
            {
                nameBuilder.Property(static n => n.Value)
                    .HasColumnName(nameof(Order.OrderName))
                    .HasMaxLength(100)
                    .IsRequired();
            });

        _ = builder.ComplexProperty(
            o => o.ShippingAddress, static addressBuilder =>
            {
                _ = addressBuilder.Property(static a => a.FirstName)
                    .HasMaxLength(50)
                    .IsRequired();
                _ = addressBuilder.Property(static a => a.LastName)
                    .HasMaxLength(50)
                    .IsRequired();
                _ = addressBuilder.Property(static a => a.Email)
                    .HasMaxLength(100)
                    .IsRequired();
                _ = addressBuilder.Property(static a => a.AddressLine)
                    .HasMaxLength(200)
                    .IsRequired();
                _ = addressBuilder.Property(static a => a.City)
                    .HasMaxLength(100)
                    .IsRequired();
                _ = addressBuilder.Property(static a => a.State)
                    .HasMaxLength(50)
                    .IsRequired();
                _ = addressBuilder.Property(static a => a.ZipCode)
                    .HasMaxLength(20)
                    .IsRequired();
                _ = addressBuilder.Property(static a => a.Country)
                    .HasMaxLength(50)
                    .IsRequired();
            });

        _ = builder.ComplexProperty(
            o => o.BillingAddress, static addressBuilder =>
            {
                _ = addressBuilder.Property(static a => a.FirstName)
                    .HasMaxLength(50)
                    .IsRequired();
                _ = addressBuilder.Property(static a => a.LastName)
                    .HasMaxLength(50)
                    .IsRequired();
                _ = addressBuilder.Property(static a => a.Email)
                    .HasMaxLength(100)
                    .IsRequired();
                _ = addressBuilder.Property(static a => a.AddressLine)
                    .HasMaxLength(200)
                    .IsRequired();
                _ = addressBuilder.Property(static a => a.City)
                    .HasMaxLength(100)
                    .IsRequired();
                _ = addressBuilder.Property(static a => a.State)
                    .HasMaxLength(50)
                    .IsRequired();
                _ = addressBuilder.Property(static a => a.ZipCode)
                    .HasMaxLength(20)
                    .IsRequired();
                _ = addressBuilder.Property(static a => a.Country)
                    .HasMaxLength(50)
                    .IsRequired();
            });

        _ = builder.ComplexProperty(
            o => o.Payment, static paymentBuilder =>
            {
                _ = paymentBuilder.Property(static p => p.CardName)
                    .HasMaxLength(50);
                _ = paymentBuilder.Property(static p => p.CardNumber)
                    .HasMaxLength(20)
                    .IsRequired();
                _ = paymentBuilder.Property(static p => p.ExpirationDate)
                    .HasMaxLength(10)
                    .IsRequired();
                _ = paymentBuilder.Property(static p => p.Cvv)
                    .HasMaxLength(3)
                    .IsRequired();
                _ = paymentBuilder.Property(static p => p.PaymentMethod);
            });

        _ = builder.Property(o => o.Status)
            .HasDefaultValue(OrderStatus.Draft)
            .HasConversion(
                s => s.ToString(),
                static dbStatus => Enum.Parse<OrderStatus>(dbStatus));

        _ = builder.Property(static o => o.TotalPrice);
    }
}
