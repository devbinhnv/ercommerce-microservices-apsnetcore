using FluentValidation;
using FluentValidation.Results;

namespace Ordering.Application.Features.V1.Commands;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public override Task<ValidationResult> ValidateAsync(ValidationContext<CreateOrderCommand> context, CancellationToken cancellation = default)
    {
        var instance = context.InstanceToValidate;
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username is required")
            .MaximumLength(150).WithMessage("Username must not exceed 150 characters.");

        RuleFor(x => x.EmailAddress)
            .EmailAddress().WithMessage($"{instance.EmailAddress} is invalid Email format.");

        RuleFor(x => x.TotalPrice)
            .NotEmpty().WithMessage("Total is required")
            .GreaterThan(0).WithMessage("Total price is greater than 0.");

        return base.ValidateAsync(context, cancellation);
    }
}
