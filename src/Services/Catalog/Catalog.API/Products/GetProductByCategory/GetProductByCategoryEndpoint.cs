
namespace Catalog.API.Products.GetProductByCategory;

public record GetProductByCategoryResponse(IEnumerable<Product> Products);

public class GetProductByCategoryEndpoint : ICarterModule
{
    private const string Route = "/products/category";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        _ = app.MapGet(pattern: $"{Route}/{{category}}",
            handler: static async (string category, ISender sender) =>
            {
                GetProductByCategoryResult result = await sender.Send(new GetProductByCategoryQuery(category));
                GetProductByCategoryResponse response = result.Adapt<GetProductByCategoryResponse>();

                return Results.Ok(response);
            })
            .WithName("GetProductByCategory")
            .Produces<GetProductByCategoryResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get products by category")
            .WithDescription("Retrieves products by category");
    }
}
