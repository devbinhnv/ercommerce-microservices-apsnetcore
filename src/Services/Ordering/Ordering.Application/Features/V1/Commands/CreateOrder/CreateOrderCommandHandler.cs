using AutoMapper;
using FluentValidation;
using MediatR;
using Ordering.Application.Common.Interfaces;
using Ordering.Domain.Entities;
using Serilog;
using Shared.SeedWork;
using ValidationException = Ordering.Application.Common.Exceptions.ValidationException;

namespace Ordering.Application.Features.V1.Commands;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ApiResult<long>>
{
    private readonly IOrderRepository _repository;
    private readonly IValidator<CreateOrderCommand> _validator;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public CreateOrderCommandHandler(
        IOrderRepository repository,
        IValidator<CreateOrderCommand> validator,
        IMapper mapper,
        ILogger logger)
    {
        _repository = repository;
        _validator = validator;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task<ApiResult<long>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.Information($"START: CreateOrderCommandHandler with order {request.Id}");
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (validationResult.Errors.Any())
            {
                throw new ValidationException(validationResult.Errors);
            }

            var newOrder = _mapper.Map<OrderEntity>(request);
            await _repository.CreateAsync(newOrder);
            await _repository.SaveChangeAsync();

            _logger.Information($"END: CreateOrderCommandHandler with order {request.Id}");
            return new ApiSuccessResult<long>(newOrder.Id, "Create succeed");
        }
        catch (ValidationException validationEx)
        {
            var errors = validationEx.Errors
                    .Select(e => $"Key: \"{e.Key}\": {e.Value}")
                    .ToList();
            return new ApiErrorResult<long>(errors);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"An occurring error in CreateOrderCommandHandler with order {request.Id}");
            throw;
        }
    }
}
