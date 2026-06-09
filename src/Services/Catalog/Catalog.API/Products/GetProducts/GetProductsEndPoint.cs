
namespace Catalog.API.Products.GetProducts;

public record GetProductsResponse(IEnumerable<Product> Products);

public class GetProductsEndPoint : ICarterModule
{
    private const string Route = "/products";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        _ = app.MapGet(
            pattern: Route,
            handler: static async (ISender sender) =>
            {
                GetProductsResult result = await sender.Send(new GetProductsQuery());
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
