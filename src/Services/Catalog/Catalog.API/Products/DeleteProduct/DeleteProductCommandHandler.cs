namespace Catalog.API.Products.DeleteProduct;

public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;

public record DeleteProductResult(bool IsSuccess);

internal class DeleteProductCommandHandler
    (IDocumentSession session, ILogger<DeleteProductCommandHandler> logger)
    : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken ct)
    {
        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation("DeleteProductCommandHandler called with Id: {@Command}", command);

        session.Delete<Product>(command.Id);
        await session.SaveChangesAsync(ct);

        return new DeleteProductResult(IsSuccess: true);
    }
}
