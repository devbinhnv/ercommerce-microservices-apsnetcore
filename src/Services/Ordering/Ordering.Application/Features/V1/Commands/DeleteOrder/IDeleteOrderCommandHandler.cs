using MediatR;

namespace Ordering.Application.Features.V1.Commands.DeleteOrder
{
    public interface IDeleteOrderCommandHandler
    {
        Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken);
    }
}