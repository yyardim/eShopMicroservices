using Ordering.Application.Orders.Commands.CreateOrder;

namespace Ordering.API.Endpoints;

public record CreateOrderRequest(OrderDto Order);
public record CreateOrderResponse(Guid Id);

public class CreateOrder : ICarterModule
{
    private const string Route = "/orders";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        _ = app.MapPost(pattern: Route,
            handler: static async (CreateOrderRequest request, ISender sender) =>
        {
            CreateOrderCommand command = request.Adapt<CreateOrderCommand>();
            CreateOrderResult result = await sender.Send(command);
            CreateOrderResponse response = result.Adapt<CreateOrderResponse>();

            return Results.Created($"{Route}/{response.Id}", response);
        })
            .WithName("CreateOrder")
            .Produces<CreateOrderResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Creates a new order.")
            .WithDescription("Creates a new order with the provided details.");
    }
}
