using FluentValidation;
using FluentValidation.Results;

namespace Ordering.Application.Features.V1.Commands;

public class DeleteOrderValidator : AbstractValidator<DeleteOrderCommand>
{
    public override Task<ValidationResult> ValidateAsync(ValidationContext<DeleteOrderCommand> context, CancellationToken cancellation = default)
    {
        var instance = context.InstanceToValidate;

        RuleFor(x => x.Id).GreaterThan(0)
            .WithMessage($"Id {instance.Id} is not valid");

        return base.ValidateAsync(context, cancellation);
    }
}
