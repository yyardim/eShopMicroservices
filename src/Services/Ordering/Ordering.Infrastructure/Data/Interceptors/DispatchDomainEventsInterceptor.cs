using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Ordering.Infrastructure.Data.Interceptors;

public class DispatchDomainEventsInterceptor(IMediator mediator)
    : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        DispatchDomainEvents(eventData.Context).GetAwaiter().GetResult();
        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken ct = default)
    {
        await DispatchDomainEvents(eventData.Context);

        return await base.SavingChangesAsync(eventData, result, ct);
    }

    private async Task DispatchDomainEvents(DbContext? context)
    {
        if (context is null) return;

        IEnumerable<IAggregate> aggregates = context.ChangeTracker
            .Entries<IAggregate>()
            .Where(a => a.Entity.DomainEvents.Any())
            .Select(a => a.Entity);

        List<IDomainEvent> domainEvents = [..
            aggregates.SelectMany(a => a.DomainEvents)];

        aggregates.ToList().ForEach(a => a.ClearDomainEvents());

        foreach (IDomainEvent domainEvent in domainEvents)
            await mediator.Publish(domainEvent);
    }
}
