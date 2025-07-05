using FluentValidation;
using FluentValidation.Results;

namespace Ordering.Application.Features.V1.Commands
{
    public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public override Task<ValidationResult> ValidateAsync(ValidationContext<UpdateOrderCommand> context, CancellationToken cancellation = default)
        {
            var instance = context.InstanceToValidate;

            RuleFor(x => x.Id).GreaterThan(0)
                .WithMessage($"Id {instance.Id} is not valid");

            RuleFor(x => x.EmailAddress)
                .EmailAddress().WithMessage($"{instance.EmailAddress} is invalid Email format.");

            RuleFor(x => x.TotalPrice)
                .NotEmpty().WithMessage("Total is required")
                .GreaterThan(0).WithMessage("Total price is greater than 0.");

            return base.ValidateAsync(context, cancellation);
        }
    }
}
