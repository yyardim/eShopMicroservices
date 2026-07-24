using FluentValidation;
using SharedKernel.Pagination;

namespace Ordering.Application.Orders.Queries.GetOrders;

public record GetOrdersQuery(PaginationRequest PaginationRequest)
    : IQuery<GetOrdersResult>;
public record GetOrdersResult(PaginatedResult<OrderDto> Orders);

public class GetOrdersQueryValidator : AbstractValidator<GetOrdersQuery>
{
    public GetOrdersQueryValidator()
    {
        _ = RuleFor(static x => x.PaginationRequest.PageIndex)
                .GreaterThanOrEqualTo(0)
                .WithMessage("PageIndex must be greater than or equal to 0.");
        _ = RuleFor(static x => x.PaginationRequest.PageSize)
                .GreaterThan(0)
                .WithMessage("PageSize must be greater than 0.");
    }
}

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
            .OrderBy(o => o.OrderName.Value)
            .Skip(pageIndex * pageSize)
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
