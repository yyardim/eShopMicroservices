using Ordering.Application.Orders.Queries.GetOrdersByName;

namespace Ordering.API.Endpoints;

public record GetOrdersByNameResponse(IEnumerable<OrderDto> Orders);

public class GetOrdersByName : ICarterModule
{
    private const string Route = "/orders";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        _ = app.MapGet(pattern: $"{Route}/{{orderName}}",
            handler: static async (string orderName, ISender sender) =>
        {
            GetOrdersByNameResult result = await sender.Send(new GetOrdersByNameQuery(orderName));
            GetOrdersByNameResponse response = result.Adapt<GetOrdersByNameResponse>();

            return Results.Ok(response);
        })
            .WithName("GetOrdersByName")
            .Produces<GetOrdersByNameResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Gets orders by name.")
            .WithDescription("Gets orders that match the provided name.");
    }
}
