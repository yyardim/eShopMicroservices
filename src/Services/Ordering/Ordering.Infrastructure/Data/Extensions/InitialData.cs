namespace Ordering.Infrastructure.Data.Extensions;

internal class InitialData
{
    public static IEnumerable<Customer> Customers =>
        [
            Customer.Create(CustomerId.From(new Guid("58c49479-1c3e-4f5b-9f8a-2d6e5b6c9f1a")), "John Doe", "john.doe@example.com"),
            Customer.Create(CustomerId.From(new Guid("d2f1c3e4-5b6a-4f5b-9f8a-2d6e5b6c9f1b")), "Jane Smith", "jane.smith@example.com")
        ];

    public static IEnumerable<Product> Products =>
        [
            Product.Create(ProductId.From(new Guid("a1b2c3d4-5e6f-7a8b-9c0d-1e2f3a4b5c6d")), "IPhone X", 799m),
            Product.Create(ProductId.From(new Guid("b1c2d3e4-5f6a-7b8c-9d0e-1f2a3b4c5d6e")), "Samsung Galaxy S20", 999m),
            Product.Create(ProductId.From(new Guid("c1d2e3f4-5a6b-7c8d-9e0f-1a2b3c4d5e6f")), "Google Pixel 5", 699m),
            Product.Create(ProductId.From(new Guid("d1e2f3a4-5b6c-7d8e-9f0a-1b2c3d4e5f6a")), "OnePlus 8T", 749m),
            Product.Create(ProductId.From(new Guid("e1f2a3b4-5c6d-7e8f-9a0b-1c2d3e4f5a6b")), "Sony Xperia 1 II", 1199m)
        ];
    public static IEnumerable<Order> OrdersWithItems
    {
        get
        {
            Address address1 = Address.Create("John", "Doe", "john.doe@example.com", "123 Main St", "Marina Del Rey", "CA", "12345", "USA");
            Address address2 = Address.Create("Jane", "Smith", "jane.smith@example.com", "456 Elm St", "Los Angeles", "CA", "67890", "USA");

            Payment payment1 = Payment.Create("4111111111111111","John Doe", "12/25", "123", 1);
            Payment payment2 = Payment.Create("4222222222222222", "Jane Smith", "11/24", "456", 2);

            Order order1 = Order.Create(
                OrderId.From(Guid.NewGuid()),
                CustomerId.From(new Guid("58c49479-1c3e-4f5b-9f8a-2d6e5b6c9f1a")),
                OrderName.Parse("Ord_1"),
                shippingAddress: address1,
                billingAddress: address1,
                payment: payment1);

            order1.Add(
                ProductId.From(new Guid("a1b2c3d4-5e6f-7a8b-9c0d-1e2f3a4b5c6d")),
                2,
                799m);
            order1.Add(
                ProductId.From(new Guid("b1c2d3e4-5f6a-7b8c-9d0e-1f2a3b4c5d6e")),
                1,
                999m);

            Order order2 = Order.Create(
                OrderId.From(Guid.NewGuid()),
                CustomerId.From(new Guid("d2f1c3e4-5b6a-4f5b-9f8a-2d6e5b6c9f1b")),
                OrderName.Parse("Ord_2"),
                shippingAddress: address2,
                billingAddress: address2,
                payment: payment2);

            order2.Add(
                ProductId.From(new Guid("c1d2e3f4-5a6b-7c8d-9e0f-1a2b3c4d5e6f")),
                1,
                699m);
            order2.Add(
                ProductId.From(new Guid("d1e2f3a4-5b6c-7d8e-9f0a-1b2c3d4e5f6a")),
                2,
                749m);

            return [order1, order2];
        }
    }
}
