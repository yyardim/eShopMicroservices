using FluentValidation;

namespace Ordering.Application.Orders.Commands.DeleteOrder;

public record DeleteOrderCommand(Guid OrderId)
    : ICommand<DeleteOrderResult>;
public record DeleteOrderResult(bool IsSuccess);

public class  DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
{
    public DeleteOrderCommandValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty().WithMessage("OrderId is required.");
    }
}

public class DeleteOrderCommandHandler(IApplicationDbContext dbContext)
    : ICommandHandler<DeleteOrderCommand, DeleteOrderResult>
{
    public async Task<DeleteOrderResult> Handle(DeleteOrderCommand command, CancellationToken ct)
    {
        OrderId orderId = OrderId.Of(command.OrderId);
        Order? order = await dbContext.Orders
            .FindAsync([orderId], cancellationToken: ct)
            ?? throw new OrderNotFoundException(command.OrderId);

        dbContext.Orders.Remove(order);
        _ = await dbContext.SaveChangesAsync(ct);

        return new DeleteOrderResult(true);
    }
}
