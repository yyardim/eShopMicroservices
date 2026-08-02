namespace Basket.API.Basket.CheckoutBasket;

public record CheckoutBasketRequest(BasketCheckoutDto BasketCheckoutDto);
public record CheckoutBasketResponse(bool IsSuccess);

public class CheckoutBasketEndpoint : ICarterModule
{
    private const string Route = "/basket/checkout";
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        _ = app.MapPost(pattern: $"{Route}",
            handler: static async (CheckoutBasketRequest request, ISender sender) =>
        {
            BasketCheckoutCommand command = request.Adapt<BasketCheckoutCommand>();
            BasketCheckoutResult result = await sender.Send(command);
            CheckoutBasketResponse response = result.Adapt<CheckoutBasketResponse>();

            return Results.Ok(response);
        })
            .WithName("CheckoutBasket")
            .Produces<CheckoutBasketResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Checkout Basket")
            .WithDescription("Checkout Basket");
    }
}
