using Ordering.Application.Extensions;

namespace Ordering.Application.Orders.Queries.GetOrdersByName;

public record GetOrdersByNameQuery(string Name)
    : IQuery<GetOrdersByNameResult>;
public record GetOrdersByNameResult(IEnumerable<OrderDto> Orders);

public class GetOrdersByNameQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetOrdersByNameQuery, GetOrdersByNameResult>
{
    public async Task<GetOrdersByNameResult> Handle(GetOrdersByNameQuery query, CancellationToken ct)
    {
        List<Order> orders = await dbContext.Orders
            .Include(o => o.OrderItems)
            .AsNoTracking()
            .Where(o => o.OrderName.Value.Contains(query.Name))
            .OrderBy(o => o.OrderName)
            .ToListAsync(ct);

        List<OrderDto> orderDtos = [.. orders.ToOrderDtoList()];
        return new GetOrdersByNameResult(orderDtos);
    }
}
