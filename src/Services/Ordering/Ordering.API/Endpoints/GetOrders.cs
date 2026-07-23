using Ordering.Application.Orders.Queries.GetOrders;
using SharedKernel.Pagination;

namespace Ordering.API.Endpoints;

public record GetOrdersResponse(PaginatedResult<OrderDto> Orders);

public class GetOrders : ICarterModule
{
    private const string Route = "/orders";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        _ = app.MapGet(pattern: Route,
            handler: static async ([AsParameters] PaginationRequest request, ISender sender) =>
        {
            GetOrdersResult result = await sender.Send(new GetOrdersQuery(request));
            GetOrdersResponse response = result.Adapt<GetOrdersResponse>();

            return Results.Ok(response);
        })
            .WithName("GetOrders")
            .Produces<GetOrdersResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Gets all orders.")
            .WithDescription("Gets a paginated list of all orders.");
    }
}
