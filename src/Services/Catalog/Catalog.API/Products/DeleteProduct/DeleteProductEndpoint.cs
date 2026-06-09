namespace Catalog.API.Products.DeleteProduct;

public record DeleteProductResponse(bool IsSuccess);

public class DeleteProductEndpoint : ICarterModule
{
    private const string Route = "/products";
    
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete(pattern: $"{Route}/{{id}}",
            handler: static async (Guid id, ISender sender) =>
            {
                DeleteProductCommand command = new(id);
                DeleteProductResult result = await sender.Send(command);
                DeleteProductResponse response = result.Adapt<DeleteProductResponse>();

                return Results.Ok(response);
            })
            .WithName("DeleteProduct")
            .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete Product")
            .WithDescription("Delete Product");
    }
}
