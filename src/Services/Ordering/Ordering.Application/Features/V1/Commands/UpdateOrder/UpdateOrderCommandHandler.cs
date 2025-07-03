using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Common.Exceptions;
using Ordering.Application.Common.Interfaces;
using Ordering.Domain.Entities;
using Serilog;
using Shared.SeedWork;
using ValidationException = Ordering.Application.Common.Exceptions.ValidationException;

namespace Ordering.Application.Features.V1.Commands
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, ApiResult<long>>
    {
        private readonly IOrderRepository _repository;
        private readonly IValidator<UpdateOrderCommand> _validator;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public UpdateOrderCommandHandler(
            IOrderRepository repository,
            IValidator<UpdateOrderCommand> validator,
            IMapper mapper,
            ILogger logger)
        {
            _repository = repository;
            _validator = validator;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResult<long>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"START: UpdateOrderCommandHandler with order {request.Id}");
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (validationResult.Errors.Any())
                {
                    throw new ValidationException(validationResult.Errors);
                }

                var orderEntity = await _repository.FindByCondition(o => o.Id == request.Id).SingleOrDefaultAsync();
                if (orderEntity == null)
                {
                    throw new NotFoundException(nameof(orderEntity), request.Id);
                }

                var updatingOrder = _mapper.Map(
                    source: request, 
                    destination: orderEntity);
                await _repository.UpdateAsync(updatingOrder);
                await _repository.SaveChangeAsync();

                _logger.Information($"END: UpdateOrderCommandHandler with order {request.Id}");
                return new ApiSuccessResult<long>(updatingOrder.Id, "Update succeed");
            }
            catch (ValidationException validationEx)
            {
                var errors = validationEx.Errors
                    .Select(e => $"Key: \"{e.Key}\": {e.Value}")
                    .ToList();
                return new ApiErrorResult<long>(errors);
            }
            catch (NotFoundException notFoundEx)
            {
                return new ApiErrorResult<long>(notFoundEx.Message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"An occurring error in UpdateOrderCommandHandler with order {request.Id}");
                throw;
            }
        }
    }
}
