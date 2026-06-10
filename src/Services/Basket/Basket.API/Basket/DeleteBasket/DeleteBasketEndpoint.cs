
namespace Basket.API.Basket.DeleteBasket;

public record DeleteBasketResponse(bool IsSuccess);

public class DeleteBasketEndpoint : ICarterModule
{
    private const string Route = "/basket";
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        _ = app.MapDelete(pattern: $"{Route}/{{userName}}",
            handler: static async (string userName, ISender sender) =>
            {
                DeleteBasketResult result = await sender.Send(new DeleteBasketCommand(userName));
                DeleteBasketResponse response = result.Adapt<DeleteBasketResponse>();
                
                return Results.Ok(response);
            })
            .WithName("DeleteBasket")
            .Produces<DeleteBasketResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Delete a basket")
            .WithDescription("Delete a basket for a user");
    }
}
