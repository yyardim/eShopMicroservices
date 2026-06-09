
namespace Catalog.API.Products.GetProducts;

public record GetProductsQuery() : IQuery<GetProductsResult>;
public record GetProductsResult(IReadOnlyList<Product> Products);

internal class GetProductsQueryHandler
    (IDocumentSession session)
    : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(
        GetProductsQuery query, CancellationToken ct)
    {
        IReadOnlyList<Product> products = await session
            .Query<Product>()
            .ToListAsync(ct);

        return new GetProductsResult(products);
    }
}
