using FluentValidation;
using OmniFit.Application.DTOs;

namespace OmniFit.Application.Validators
{
    public class WorkoutSetRequestValidator : AbstractValidator<WorkoutSetRequest>
    {
        public WorkoutSetRequestValidator()
        {
            RuleFor(x => x.Reps)
                .GreaterThanOrEqualTo(0).WithMessage("Reps cannot be negative");

            RuleFor(x => x.Weight)
                .GreaterThanOrEqualTo(0).WithMessage("Weight cannot be negative");
        }
    }
}
