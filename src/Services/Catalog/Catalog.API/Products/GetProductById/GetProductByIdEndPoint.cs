
namespace Catalog.API.Products.GetProductById;

public record GetProductByIdResponse(Product Product);

public class GetProductByIdEndPoint : ICarterModule
{
    private const string Route = "/products";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        _ = app.MapGet(pattern: $"{Route}/{{id}}",
            handler: static async (Guid id, ISender sender) =>
            {
                GetProductByIdResult result = await sender.Send(new GetProductByIdQuery(id));
                GetProductByIdResponse response = result.Adapt<GetProductByIdResponse>();
                
                return Results.Ok(response);
            })
            .WithName("GetProductById")
            .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Product By Id")
            .WithDescription("Get Product By Id");
    }
}
