using FluentValidation;
using FluentValidation.Results;
using MediatR;
using ValidationException =  Ordering.Application.Common.Exceptions.ValidationException;

namespace Ordering.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse> :
    IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator> _validators;

    public ValidationBehavior(IEnumerable<IValidator> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if(!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);
        var validationResuls = await Task.WhenAll(_validators.Select(
            v => v.ValidateAsync(context, cancellationToken)));

        IEnumerable<ValidationFailure> failures = validationResuls.Where(r => r.Errors.Any())
            .SelectMany(r => r.Errors.ToList());
        if(failures.Any())
        {
            throw new ValidationException(failures);
        }

        return await next();
    }
}
