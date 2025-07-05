using AutoMapper;
using MediatR;
using Ordering.Application.Common.Interfaces;
using Serilog;


namespace Ordering.Application.Features.V1.Commands.DeleteOrder;

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, Unit>, IDeleteOrderCommandHandler
{
    private readonly IOrderRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public DeleteOrderCommandHandler(
        IOrderRepository repository,
        IMapper mapper,
        ILogger logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.Information($"START: DeleteOrderCommandHandler with order {request.Id}");

            var deletingOrder = await _repository.GetByIdAsync(request.Id);
            if (deletingOrder != null)
            {
                _repository.DeleteAsync(deletingOrder);
                _repository.SaveChangeAsync();
            }

            _logger.Information($"END: DeleteOrderCommandHandler with order {request.Id}");
            return Unit.Value;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"An occurring error in DeleteOrderCommandHandler with order {request.Id}");
            throw;
        }
    }
}
