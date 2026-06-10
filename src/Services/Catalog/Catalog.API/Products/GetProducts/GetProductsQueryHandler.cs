namespace Catalog.API.Products.GetProducts;

public record GetProductsQuery(int? PageNumber = 1, int? PageSize = 10) : IQuery<GetProductsResult>;
public record GetProductsResult(IEnumerable<Product> Products);

internal class GetProductsQueryHandler(IDocumentSession session)
    : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken ct)
    {
        int pageNumber = Math.Max(1, query.PageNumber ?? 1);
        int pageSize = Math.Clamp(query.PageSize ?? 10, 1, 1000);

        IPagedList<Product> products = await session
            .Query<Product>()
            .ToPagedListAsync(pageNumber, pageSize, ct);

        return new GetProductsResult(products);
    }
}
