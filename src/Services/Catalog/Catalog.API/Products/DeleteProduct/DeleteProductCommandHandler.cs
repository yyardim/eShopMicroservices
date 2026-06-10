namespace Catalog.API.Products.DeleteProduct;

public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;
public record DeleteProductResult(bool IsSuccess);

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        _ = RuleFor(static x => x.Id).NotEmpty().WithMessage("Id required!");
    }
}

internal class DeleteProductCommandHandler
    (IDocumentSession session)
    : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken ct)
    {
        session.Delete<Product>(command.Id);
        await session.SaveChangesAsync(ct);

        return new DeleteProductResult(IsSuccess: true);
    }
}
