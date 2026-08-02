using MassTransit;
using Ordering.Application.Orders.Commands.CreateOrder;
using Ordering.Domain.Enums;
using SharedKernel.Messaging.Events;

namespace Ordering.Application.Orders.EventHandlers.Integration;

public class BasketCheckoutEventHandler
    (ISender sender, ILogger<BasketCheckoutEventHandler> logger)
    : IConsumer<BasketCheckoutEvent>
{
    public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
    {
        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation("Integration Event handled: {IntegrationEvent}", context.Message.GetType().Name);

        CreateOrderCommand command = MapToCreateOrderCommand(context.Message);
        _ = await sender.Send(command, context.CancellationToken);

    }

    private static CreateOrderCommand MapToCreateOrderCommand(BasketCheckoutEvent message)
    {
        // Create order with incoming basket checkout event data
        AddressDto addressDto = new(
            message.FirstName,
            message.LastName,
            message.Email,
            message.AddressLine,
            message.City,
            message.State,
            message.ZipCode,
            message.Country);

        PaymentDto paymentDto = new(
            message.CardNumber,
            message.CardHolderName,
            message.ExpirationDate,
            message.Cvv,
            message.PaymentMethod);

        Guid orderId = Guid.NewGuid(); // Generate a new order ID

        OrderDto orderDto = new(
            Id: orderId,
            CustomerId: message.CustomerId,
            OrderName: message.UserName,
            ShippingAddress: addressDto,
            BillingAddress: addressDto,
            Payment: paymentDto,
            Status: OrderStatus.Pending,
            OrderItems: [.. message.Items.Select(item => new OrderItemDto(
                orderId,
                item.ProductId,
                item.Quantity,
                item.Price))]);

        return new CreateOrderCommand(orderDto);
    }
}
