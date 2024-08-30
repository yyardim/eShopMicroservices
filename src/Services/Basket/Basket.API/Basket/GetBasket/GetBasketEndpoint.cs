namespace Basket.API.Basket.GetBasket;

//public record GetBasketQuery(string UserName);
public record GetBasketResponse(ShoppingCart Cart);

public class GetBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/basket/{userName}",
            async (string userName, ISender sender) =>
            {
                GetBasketResult result = await sender.Send(new GetBasketQuery(userName));

                GetBasketResponse response = result.Adapt<GetBasketResponse>();

                return Results.Ok(response);
            })
            .WithName("GetProductById")
            .Produces<GetBasketResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Product by Id")
            .WithDescription("Get a product by its unique identifier.");
    }
}
