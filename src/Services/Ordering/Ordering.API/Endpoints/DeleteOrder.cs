using Ordering.Application.Orders.Commands.DeleteOrder;

namespace Ordering.API.Endpoints;

public record DeleteOrderResponse(bool IsSuccess);

public class DeleteOrder : ICarterModule
{
    private const string Route = "/orders";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        _ = app.MapDelete(pattern: $"{Route}/{{id:guid}}",
            handler: static async (Guid id, ISender sender) =>
        {
            DeleteOrderResult result = await sender.Send(new DeleteOrderCommand(id));
            DeleteOrderResponse response = result.Adapt<DeleteOrderResponse>();

            return Results.Ok(response);
        })
            .WithName("DeleteOrder")
            .Produces<DeleteOrderResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Deletes an existing order.")
            .WithDescription("Deletes an existing order with the provided ID.");
    }
}
