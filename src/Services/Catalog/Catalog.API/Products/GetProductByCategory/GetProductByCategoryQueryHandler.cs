
namespace Catalog.API.Products.GetProductByCategory;

public record GetProductByCategoryQuery(string Category)
    : IQuery<GetProductByCategoryResult>;
public record GetProductByCategoryResult(IReadOnlyList<Product> Products);

internal class GetProductByCategoryQueryHandler
    (IDocumentSession session)
    : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
{
    public async Task<GetProductByCategoryResult> Handle(
        GetProductByCategoryQuery query, 
        CancellationToken ct)
    {
        IReadOnlyList<Product> products = await session.Query<Product>()
            .Where(p => p.Category.Contains(query.Category))
            .ToListAsync(ct);

        return new GetProductByCategoryResult(products);
    }
}
