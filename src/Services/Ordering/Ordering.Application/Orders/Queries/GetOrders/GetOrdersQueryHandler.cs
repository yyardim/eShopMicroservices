using SharedKernel.Pagination;

namespace Ordering.Application.Orders.Queries.GetOrders;

public record GetOrdersQuery(PaginationRequest PaginationRequest)
    : IQuery<GetOrdersResult>;
public record GetOrdersResult(PaginatedResult<OrderDto> Orders);

public class GetOrdersQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetOrdersQuery, GetOrdersResult>
{
    public async Task<GetOrdersResult> Handle(GetOrdersQuery query, CancellationToken ct)
    {
        int pageIndex = query.PaginationRequest.PageIndex;
        int pageSize = query.PaginationRequest.PageSize;

        long totalCount = await dbContext.Orders.LongCountAsync(ct);

        List<Order> orders = await dbContext.Orders
            .Include(o => o.OrderItems)
            .AsNoTracking()
            .OrderBy(o => o.OrderName)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new GetOrdersResult(
            new PaginatedResult<OrderDto>(
                pageIndex,
                pageSize,
                totalCount,
                orders.ToOrderDtoList()
            ));
    }
}
