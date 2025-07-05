using AutoMapper;
using Infrastructure.Mappings;
using MediatR;
using Ordering.Application.Common.Mappings;
using Ordering.Domain.Entities;
using Ordering.Domain.Enums;
using Shared.SeedWork;

namespace Ordering.Application.Features.V1.Commands;

public class UpdateOrderCommand :CreateOrderCommand, IRequest<ApiResult<long>>, IMapFrom<OrderEntity>
{
    public void SetId(long id)
    {
        Id = id;
    }

    public long Id { get; private set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateOrderCommand, OrderEntity>()
            .IgnoreAllNonExisting()
            .IgnoreNullProperties()
            // Do not allow change order status, only change status through consumer
            .ForMember(dest => dest.Status, opt => opt.Ignore());
    }
}
