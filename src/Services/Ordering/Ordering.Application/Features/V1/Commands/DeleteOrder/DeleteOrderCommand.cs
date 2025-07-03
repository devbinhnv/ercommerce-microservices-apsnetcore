using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Shared.SeedWork;

namespace Ordering.Application.Features.V1.Commands;

public class DeleteOrderCommand : IRequest<ApiResult<NoContent>>
{
    public long Id { get; private set; }

    public DeleteOrderCommand(long id)
    {
        Id = id;
    }
}
