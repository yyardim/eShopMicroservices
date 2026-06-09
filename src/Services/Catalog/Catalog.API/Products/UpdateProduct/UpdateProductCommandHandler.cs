namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductCommand(
    Guid Id, 
    string Name,
    List<string> Category,
    string Description,
    string ImageFile,
    decimal Price) 
    : ICommand<UpdateProductResult>;

public record UpdateProductResult(bool IsSuccess);

internal class UpdateProductCommandHandler
    (IDocumentSession session, ILogger<UpdateProductCommandHandler> logger)
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(
        UpdateProductCommand command, CancellationToken ct)
    {

        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation("UpdateProductCommandHandler called with {@Command}", command);

        Product? product = await session.LoadAsync<Product>(command.Id, ct) 
            ?? throw new ProductNotFoundException(command.Id);

        product.Name = command.Name;
        product.Category = command.Category;
        product.Description = command.Description;
        product.ImageFile = command.ImageFile;
        product.Price = command.Price;

        session.Update(product);
        await session.SaveChangesAsync(ct);

        return new UpdateProductResult(true);
    }
}
