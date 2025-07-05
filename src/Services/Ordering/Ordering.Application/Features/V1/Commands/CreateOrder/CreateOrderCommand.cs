using AutoMapper;
using EvenBus.Messages.IntegrationEvents.Events;
using Infrastructure.Mappings;
using MediatR;
using Ordering.Application.Common.Mappings;
using Ordering.Domain.Entities;
using Shared.SeedWork;

namespace Ordering.Application.Features.V1.Commands;

public class CreateOrderCommand : CreateOrUpdateCommand, IRequest<ApiResult<long>>, IMapFrom<OrderEntity>
{
    public string UserName { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateOrderCommand, OrderEntity>()
            .IgnoreAllNonExisting()
            .IgnoreNullProperties();
        profile.CreateMap<BasketCheckoutEvent, CreateOrderCommand>();
    }
}
