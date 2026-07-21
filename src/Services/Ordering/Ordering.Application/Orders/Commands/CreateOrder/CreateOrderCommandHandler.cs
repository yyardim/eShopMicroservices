using FluentValidation;

namespace Ordering.Application.Orders.Commands.CreateOrder;

public record CreateOrderCommand(OrderDto Order) : ICommand<CreateOrderResult>;
public record CreateOrderResult(Guid Id);

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.Order.OrderName).NotEmpty().WithMessage("Name is required.");
        RuleFor(x => x.Order.CustomerId).NotEmpty().WithMessage("CustomerId is required");
        RuleFor(x => x.Order.OrderItems).NotEmpty().WithMessage("Order Items should not be empty");
    }
}

public class CreateOrderCommandHandler(IApplicationDbContext dbContext)
    : ICommandHandler<CreateOrderCommand, CreateOrderResult>
{
    public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken ct)
    {

        Order order = CreateNewOrder(command.Order);

        dbContext.Orders.Add(order);
        _ = await dbContext.SaveChangesAsync(ct);

        return new CreateOrderResult(order.Id.Value);
    }

    private static Order CreateNewOrder(OrderDto orderDto)
    {
        Address shippingAddress = Address.Of(
            orderDto.ShippingAddress.FirstName,
            orderDto.ShippingAddress.LastName,
            orderDto.ShippingAddress.Email,
            orderDto.ShippingAddress.AddressLine,
            orderDto.ShippingAddress.City,
            orderDto.ShippingAddress.State,
            orderDto.ShippingAddress.ZipCode,
            orderDto.ShippingAddress.Country);

        Address billingAddress = Address.Of(
            orderDto.BillingAddress.FirstName,
            orderDto.BillingAddress.LastName,
            orderDto.BillingAddress.Email,
            orderDto.BillingAddress.AddressLine,
            orderDto.BillingAddress.City,
            orderDto.BillingAddress.State,
            orderDto.BillingAddress.ZipCode,
            orderDto.BillingAddress.Country);

        Order newOrder = Domain.Models.Order.Create(
            id: OrderId.Of(Guid.NewGuid()),
            customerId: CustomerId.Of(orderDto.CustomerId),
            orderName: OrderName.Of(orderDto.OrderName),
            shippingAddress: shippingAddress,
            billingAddress: billingAddress,
            payment: Payment.Of(
                orderDto.Payment.CardName,
                orderDto.Payment.CardNumber,
                orderDto.Payment.CardHolderName,
                orderDto.Payment.ExpirationDate,
                orderDto.Payment.Cvv,
                orderDto.Payment.PaymentMethod));

        foreach (var orderItemDto in orderDto.OrderItems)
            newOrder.Add(
                ProductId.Of(orderItemDto.ProductId),
                orderItemDto.Quantity,
                orderItemDto.Price);

        return newOrder;
    }
}
