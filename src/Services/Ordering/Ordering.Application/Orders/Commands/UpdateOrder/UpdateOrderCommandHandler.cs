using FluentValidation;

namespace Ordering.Application.Orders.Commands.UpdateOrder;

public record UpdateOrderCommand(OrderDto Order) : ICommand<UpdateOrderResult>;
public record UpdateOrderResult(bool IsSuccess);

public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(x => x.Order.Id).NotEmpty().WithMessage("Id is required.");
        _ = RuleFor(x => x.Order.OrderName)
            .NotEmpty().WithMessage("Name is required.")
            .MinimumLength(2).WithMessage("Name must be at least 2 characters.");
        RuleFor(x => x.Order.CustomerId).NotEmpty().WithMessage("CustomerId is required");
    }
}

public class UpdateOrderCommandHandler(IApplicationDbContext dbContext)
    : ICommandHandler<UpdateOrderCommand, UpdateOrderResult>
{
    public async Task<UpdateOrderResult> Handle(UpdateOrderCommand command, CancellationToken ct)
    {
        OrderId orderId = OrderId.From(command.Order.Id);
        Order? order = await dbContext.Orders
            .FindAsync(orderId, ct)
            ?? throw new OrderNotFoundException(command.Order.Id);

        UpdateOrderWithNewValues(order, command.Order);

        dbContext.Orders.Update(order);
        _ = await dbContext.SaveChangesAsync(ct);

        return new UpdateOrderResult(true);
    }

    private static void UpdateOrderWithNewValues(Order order, OrderDto orderDto)
    {
        Address updatedShippingAddress = Address.Create(
            orderDto.ShippingAddress.FirstName,
            orderDto.ShippingAddress.LastName,
            orderDto.ShippingAddress.Email,
            orderDto.ShippingAddress.AddressLine,
            orderDto.ShippingAddress.City,
            orderDto.ShippingAddress.State,
            orderDto.ShippingAddress.ZipCode,
            orderDto.ShippingAddress.Country);

        Address updatedBillingAddress = Address.Create(
            orderDto.BillingAddress.FirstName,
            orderDto.BillingAddress.LastName,
            orderDto.BillingAddress.Email,
            orderDto.BillingAddress.AddressLine,
            orderDto.BillingAddress.City,
            orderDto.BillingAddress.State,
            orderDto.BillingAddress.ZipCode,
            orderDto.BillingAddress.Country);

        Payment updatedPayment = Payment.Create(
            orderDto.Payment.CardNumber,
            orderDto.Payment.CardHolderName,
            orderDto.Payment.ExpirationDate,
            orderDto.Payment.Cvv,
            orderDto.Payment.PaymentMethod);

        order.Update(
            orderName: OrderName.Parse(orderDto.OrderName),
            shippingAddress: updatedShippingAddress,
            billingAddress: updatedBillingAddress,
            payment: updatedPayment,
            status: orderDto.Status);
    }
}
