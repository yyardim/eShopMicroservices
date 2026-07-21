namespace Ordering.Application.Orders.EventHandlers;

public class OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> logger)
    : INotificationHandler<OrderCreatedEvent>
{
    public Task Handle(OrderCreatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain event handled: {DomainEvent}", notification.GetType().Name);
        return Task.CompletedTask;
    }
}
