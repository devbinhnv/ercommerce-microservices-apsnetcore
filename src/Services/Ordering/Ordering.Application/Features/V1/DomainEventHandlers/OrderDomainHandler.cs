using MediatR;
using Ordering.Domain.OrderAggregate.Events;
using Serilog;

namespace Ordering.Application.Features.V1.DomainEventHandlers;

public class OrderDomainHandler :
    INotificationHandler<OrderCreatedEvent>,
    INotificationHandler<OrderDeletedEvent>

{
    private readonly ILogger _logger;

    public OrderDomainHandler(ILogger logger)
    {
        _logger = logger;
    }

    public Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        // Complete domain logic here
        _logger.Information("Ordering domain event: {domainEvent}", notification.GetType().Name);
        return Task.CompletedTask;
    }

    public Task Handle(OrderDeletedEvent notification, CancellationToken cancellationToken)
    {
        // Complete domain logic here
        _logger.Information("Ordering domain event: {domainEvent}", notification.GetType().Name);
        return Task.CompletedTask;
    }
}
