﻿namespace Catalog.API.Products.CreateProduct;

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
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name required!");
        RuleFor(x => x.Category).NotEmpty().WithMessage("Category required!");
        RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile required!");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price not valid!");
    }
}

internal class CreateProductCommandHandler
    (IDocumentSession session) 
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(
        CreateProductCommand command, 
        CancellationToken cancellationToken)
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
        await session.SaveChangesAsync(cancellationToken);

        return new CreateProductResult(product.Id);
    }
}
