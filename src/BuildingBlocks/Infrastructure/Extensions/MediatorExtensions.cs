using Contracts.Common.Events;
using Contracts.Common.Interfaces;
using Infrastructure.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Infrastructure.Extensions;

public static class MediatorExtensions
{
    public static async Task DispatchDomainEventsAsync(this IMediator mediator, 
        List<BaseEvent> domainEvents, 
        ILogger logger)
    {
        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent);

            var eventData = new SerializeService().Seriallize(domainEvent);
            logger.Information("\n----\nA domain event has been published!\n" +
                $"Event: {domainEvent.GetType().Name}\n" +
                $"Data: {eventData}");
        }
    }
}
