namespace Catalog.API.Products.GetProductById;

public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;
public record GetProductByIdResult(Product Product);

internal class GetProductByIdQueryHandler
    (IDocumentSession session)
    : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(
        GetProductByIdQuery query, CancellationToken ct)
    {
        Product? product = await session.LoadAsync<Product>(query.Id, ct);

        return product is null 
            ? throw new ProductNotFoundException(query.Id) 
            : new GetProductByIdResult(product);
    }
}
