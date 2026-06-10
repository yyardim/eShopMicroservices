
namespace Catalog.API.Products.GetProducts;

public record GetProductsRequest(int? PageNumber = 1, int? PageSize = 10);
public record GetProductsResponse(IEnumerable<Product> Products);

public class GetProductsEndPoint : ICarterModule
{
    private const string Route = "/products";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        _ = app.MapGet(
            pattern: Route,
            handler: static async ([AsParameters] GetProductsRequest request, ISender sender) =>
            {
                GetProductsQuery query = request.Adapt<GetProductsQuery>();
                GetProductsResult result = await sender.Send(query);
                GetProductsResponse response = result.Adapt<GetProductsResponse>();
                
                return Results.Ok(response);
            })
            .WithName("GetProducts")
            .Produces<GetProductsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Products")
            .WithDescription("Get Products");
    }
}
