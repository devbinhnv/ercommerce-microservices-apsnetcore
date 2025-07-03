using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Ordering.Application.Common.Exceptions;
using Ordering.Application.Common.Interfaces;
using Serilog;
using Shared.SeedWork;
using ValidationException = Ordering.Application.Common.Exceptions.ValidationException;


namespace Ordering.Application.Features.V1.Commands.DeleteOrder;

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, ApiResult<NoContent>>
{
    private readonly IOrderRepository _repository;
    private readonly IValidator<DeleteOrderCommand> _validator;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public DeleteOrderCommandHandler(
        IOrderRepository repository,
        IValidator<DeleteOrderCommand> validator,
        IMapper mapper,
        ILogger logger)
    {
        _repository = repository;
        _validator = validator;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task<ApiResult<NoContent>> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.Information($"START: DeleteOrderCommandHandler with order {request.Id}");
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (validationResult.Errors.Any())
            {
                throw new ValidationException(validationResult.Errors);
            }

            var deletingOrder = await _repository.GetByIdAsync(request.Id);
            if (deletingOrder == null)
            {
                throw new NotFoundException(nameof(deletingOrder), request.Id);
            }

            await _repository.DeleteAsync(deletingOrder);
            await _repository.SaveChangeAsync();

            _logger.Information($"END: DeleteOrderCommandHandler with order {request.Id}");
            return new ApiResult<NoContent>();
        }
        catch (ValidationException validationEx)
        {
            var errors = validationEx.Errors
                    .Select(e => $"Key: \"{e.Key}\": {e.Value}")
                    .ToList();
            return new ApiErrorResult<NoContent>(errors);
        }
        catch (NotFoundException notFoundEx)
        {
            return new ApiErrorResult<NoContent>(notFoundEx.Message);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"An occurring error in DeleteOrderCommandHandler with order {request.Id}");
            throw;
        }
    }
}
