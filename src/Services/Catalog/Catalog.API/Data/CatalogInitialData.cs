using Marten.Schema;

namespace Catalog.API.Data;

public class CatalogInitialData : IInitialData
{
    public async Task Populate(IDocumentStore store, CancellationToken ct)
    {
        using var session = store.LightweightSession();

        if (await session.Query<Product>().AnyAsync(token: ct))
            return;

        session.Store(GetPreconfiguredProducts());
        await session.SaveChangesAsync(ct);
    }

    private static IEnumerable<Product> GetPreconfiguredProducts() =>
    [
        new Product
        {
            Id = new Guid("5334c996-8457-4c8b-9e1a-1f0e5b6c8d1a"),
            Name = "IPhone X",
            Category = ["Smart Phone"],
            Description = "This phone is the company's biggest change in design.",
            ImageFile = "product-1.png",
            Price = 950.0m
        },
        new Product
        {
            Id = new Guid("a1b2c3d4-e5f6-7890-1234-56789abcdef0"),
            Name = "Samsung 10",
            Category = ["Smart Phone"],
            Description = "This phone is the company's biggest change in design.",
            ImageFile = "product-2.png",
            Price = 840.0m
        },
        new Product
        {
            Id = new Guid("0f1e2d3c-4b5a-6789-0123-456789abcdef"),
            Name = "Huawei Plus",
            Category = ["Smart Phone"],
            Description = "This phone is the company's biggest change in design.",
            ImageFile = "product-3.png",
            Price = 650.0m
        },
        new Product
        {
            Id = new Guid("12345678-90ab-cdef-1234-567890abcdef"),
            Name = "Xiaomi Mi 9",
            Category = ["Smart Phone"],
            Description = "This phone is the company's biggest change in design.",
            ImageFile = "product-4.png",
            Price = 470.0m
        },
        new Product
        {
            Id = new Guid("abcdef12-3456-7890-abcd-ef1234567890"),
            Name = "HTC U11+ Plus",
            Category = ["Smart Phone"],
            Description = "This phone is the company's biggest change in design.",
            ImageFile = "product-5.png",
            Price = 380.0m
        },
        new Product
        {
            Id = new Guid("fedcba98-7654-3210-fedc-ba9876543210"),
            Name = "LG G7 ThinQ",
            Category = ["Smart Phone"],
            Description = "This phone is the company's biggest change in design.",
            ImageFile = "product-6.png",
            Price = 240.0m
        },
        new Product
        {
            Id = new Guid("0a1b2c3d-4e5f-6789-0123-456789abcdef"),
            Name = "Sony Xperia XZ",
            Category = ["Smart Phone"],
            Description = "This phone is the company's biggest change in design.",
            ImageFile = "product-7.png",
            Price = 500.0m
        },
        new Product
        {
            Id = new Guid("abcdef12-3456-7890-abcd-ef1234567890"),
            Name = "Nokia 7.1",
            Category = ["Smart Phone"],
            Description = "This phone is the company's biggest change in design.",
            ImageFile = "product-8.png",
            Price = 350.0m
        },
        new Product
        {
            Id = new Guid("12345678-90ab-cdef-1234-567890abcdef"),
            Name = "Apple Airpods",
            Category = ["Accessories"],
            Description = "Bluetooth technology lets you connect it with compatible devices wirelessly.",
            ImageFile = "product-9.png",
            Price = 160.0m
        },
        new Product
        {
            Id = new Guid("0f1e2d3c-4b5a-6789-0123-456789abcdef"),
            Name = "Bose SoundSport",
            Category = ["Accessories"],
            Description = "Wireless headphones with excellent sound quality.",
            ImageFile = "product-10.png",
            Price = 120.0m
        },
        new Product
        {
            Id = new Guid("a1b2c3d4-e5f6-7890-1234-56789abcdef0"),
            Name = "Sony WH-1000XM4",
            Category = ["Accessories"],
            Description = "Industry-leading noise canceling with Dual Noise Sensor technology.",
            ImageFile = "product-11.png",
            Price = 350.0m
        },
        new Product
        {
            Id = new Guid("5334c996-8457-4c8b-9e1a-1f0e5b6c8d1a"),
            Name = "Apple Watch Series 6",
            Category = ["Wearables"],
            Description = "The future of health is on your wrist.",
            ImageFile = "product-12.png",
            Price = 400.0m
        },
        new Product
        {
            Id = new Guid("0a1b2c3d-4e5f-6789-0123-456789abcdef"),
            Name = "Fitbit Charge 4",
            Category = ["Wearables"],
            Description = "Advanced fitness tracker with built-in GPS.",
            ImageFile = "product-13.png",
            Price = 150.0m
        },
        new Product
        {
            Id = new Guid("abcdef12-3456-7890-abcd-ef1234567890"),
            Name = "Garmin Forerunner 945",
            Category = ["Wearables"],
            Description = "Premium GPS running and triathlon smartwatch.",
            ImageFile = "product-14.png",
            Price = 600.0m
        },
        new Product
        {
            Id = new Guid("fedcba98-7654-3210-fedc-ba9876543210"),
            Name = "Samsung Galaxy Watch 3",
            Category = ["Wearables"],
            Description = "The next generation of Samsung's smartwatch.",
            ImageFile = "product-15.png",
            Price = 350.0m
        },
        new Product
        {
            Id = new Guid("12345678-90ab-cdef-1234-567890abcdef"),
            Name = "Apple iPad Pro",
            Category = ["Tablets"],
            Description = "The ultimate iPad experience with the powerful M1 chip.",
            ImageFile = "product-16.png",
            Price = 800.0m
        },
        new Product
        {
            Id = new Guid("0f1e2d3c-4b5a-6789-0123-456789abcdef"),
            Name = "Microsoft Surface Pro 7",
            Category = ["Tablets"],
            Description = "The ultimate tablet experience with the powerful Intel processor.",
            ImageFile = "product-17.png",
            Price = 900.0m
        }
    ];
}
