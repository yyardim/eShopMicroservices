namespace Ordering.Application.Orders.EventHandlers;

public class OrderUpdatedEventHandler(ILogger<OrderUpdatedEventHandler> logger)
    : INotificationHandler<OrderUpdatedEvent>
{
    public Task Handle(OrderUpdatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain event handled: {DomainEvent}", notification.GetType().Name);
        return Task.CompletedTask;
    }
}
