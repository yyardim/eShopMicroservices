namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommand(string Name,
    List<string> Category, 
    string Description, 
    string ImageFile, 
    decimal Price) : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        _ = RuleFor(static x => x.Name).NotEmpty().WithMessage("Name required!");
        _ = RuleFor(static x => x.Category).NotEmpty().WithMessage("Category required!");
        _ = RuleFor(static x => x.ImageFile).NotEmpty().WithMessage("ImageFile required!");
        _ = RuleFor(static x => x.Price).GreaterThan(0).WithMessage("Price not valid!");
    }
}

internal class CreateProductCommandHandler
    (IDocumentSession session)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(
        CreateProductCommand command, 
        CancellationToken ct)
    {
        Product product = new()
        {
            Name = command.Name,
            Category = command.Category,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Price = command.Price
        };

        // Save product to database
        session.Store(product);
        await session.SaveChangesAsync(ct);

        return new CreateProductResult(product.Id);
    }
}
