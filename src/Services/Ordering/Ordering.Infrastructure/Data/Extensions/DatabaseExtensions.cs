using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Infrastructure.Data.Extensions;

public static class DatabaseExtensions
{
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();

        ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await dbContext.Database.MigrateAsync();
        await SeedAsync(dbContext);
    }

    private static async Task SeedAsync(ApplicationDbContext dbContext)
    {
        await SeedCustomerAsync(dbContext);
        await SeedProductAsync(dbContext);
        await SeedOrderAndItemsAsync(dbContext);
    }

    private static async Task SeedCustomerAsync(ApplicationDbContext dbContext)
    {
        if (!await dbContext.Customers.AnyAsync())
        {
            await dbContext.Customers.AddRangeAsync(InitialData.Customers);
            await dbContext.SaveChangesAsync();
        }
    }

    private static async Task SeedProductAsync(ApplicationDbContext dbContext)
    {
        if (!await dbContext.Products.AnyAsync())
        {
            await dbContext.Products.AddRangeAsync(InitialData.Products);
            await dbContext.SaveChangesAsync();
        }
    }

    private static async Task SeedOrderAndItemsAsync(ApplicationDbContext dbContext)
    {
        if (!await dbContext.Orders.AnyAsync())
        {
            await dbContext.Orders.AddRangeAsync(InitialData.OrdersWithItems);
            await dbContext.SaveChangesAsync();
        }
    }
}
