﻿using AutoMapper;
using MediatR;
using Ordering.Application.Common.Interfaces;
using Ordering.Application.Common.Models;
using Ordering.Application.Features.V1.Orders;
using Shared.SeedWork;

namespace Ordering.Application.Features.V1.Queries;

public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, ApiResult<List<OrderDto>>>
{
    private readonly IMapper _mapper;
    private readonly IOrderRepository _repository;

    public GetOrdersQueryHandler(IMapper mapper, IOrderRepository repository)
    {
        ArgumentNullException.ThrowIfNull(nameof(mapper));
        ArgumentNullException.ThrowIfNull(nameof(repository));

        _mapper = mapper;
        _repository = repository;
    }
    public async Task<ApiResult<List<OrderDto>>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var orderEntities = await _repository.GetOrdersByUserName(request.UserName);
        var orderList = _mapper.Map<List<OrderDto>>(orderEntities);

        return new ApiSuccessResult<List<OrderDto>>(orderList);
    }
}
