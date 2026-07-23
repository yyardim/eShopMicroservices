using Ordering.Application.Orders.Commands.UpdateOrder;

namespace Ordering.API.Endpoints;

public record UpdateOrderRequest(OrderDto Order);
public record UpdateOrderResponse(bool IsSuccess);

public class UpdateOrder : ICarterModule
{
    private const string Route = "/orders";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        _ = app.MapPut(pattern: Route,
            handler: static async (UpdateOrderRequest request, ISender sender) =>
        {
            UpdateOrderCommand command = request.Adapt<UpdateOrderCommand>();
            UpdateOrderResult result = await sender.Send(command);
            UpdateOrderResponse response = result.Adapt<UpdateOrderResponse>();

            return Results.Ok(response);
        })
            .WithName("UpdateOrder")
            .Produces<UpdateOrderResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Updates an existing order.")
            .WithDescription("Updates an existing order with the provided details.");
    }
}
