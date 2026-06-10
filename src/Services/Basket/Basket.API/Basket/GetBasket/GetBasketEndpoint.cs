namespace Basket.API.Basket.GetBasket;

public record GetBasketResponse(ShoppingCart Cart);

public class GetBasketEndpoint : ICarterModule
{
    private const string Route = "/basket";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        _ = app.MapGet(pattern: $"{Route}/{{userName}}",
            handler: static async (string userName, ISender sender) =>
            {
                GetBasketResult result = await sender.Send(new GetBasketQuery(userName));
                GetBasketResponse response = result.Adapt<GetBasketResponse>();

                return Results.Ok(response);
            })
            .WithName("GetBasketByUserName")
            .Produces<GetBasketResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Basket by UserName")
            .WithDescription("Get a basket for a user by their username.");
    }
}
