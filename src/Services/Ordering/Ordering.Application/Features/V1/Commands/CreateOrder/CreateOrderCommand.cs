using AutoMapper;
using MediatR;
using Ordering.Application.Common.Mappings;
using Ordering.Domain.Entities;
using Ordering.Domain.Enums;
using Shared.SeedWork;

namespace Ordering.Application.Features.V1.Commands;

public class CreateOrderCommand : IRequest<ApiResult<long>>, IMapFrom<OrderEntity>
{
    public long Id { get; set; }

    public string UserName { get; set; }

    public decimal TotalPrice { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    // Address
    public string EmailAddress { get; set; }

    public string ShippingAddress { get; set; }

    public string InvoiceAddress { get; set; }

    public EOrderStatus Status { get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateOrderCommand, OrderEntity>();
    }
}
