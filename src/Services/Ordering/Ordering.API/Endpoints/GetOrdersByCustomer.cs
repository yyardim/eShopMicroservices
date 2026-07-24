using Ordering.Application.Orders.Queries.GetOrdersByCustomer;

namespace Ordering.API.Endpoints;

public record GetOrdersByCustomerResponse(IEnumerable<OrderDto> Orders);

public class GetOrdersByCustomer : ICarterModule
{
    private const string Route = "/orders/customer";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        _ = app.MapGet(pattern: $"{Route}/{{customerId:guid}}",
            handler: static async (Guid customerId, ISender sender) =>
        {
            GetOrdersByCustomerResult result = await sender.Send(
                new GetOrdersByCustomerQuery(customerId));
            GetOrdersByCustomerResponse response = result.Adapt<GetOrdersByCustomerResponse>();

            return Results.Ok(response);
        })
            .WithName("GetOrdersByCustomer")
            .Produces<GetOrdersByCustomerResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Gets orders by customer.")
            .WithDescription("Gets orders that belong to the specified customer.");
    }
}
