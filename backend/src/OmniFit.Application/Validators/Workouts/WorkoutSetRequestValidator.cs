using FluentValidation;
using OmniFit.Application.DTOs.Workouts;

namespace OmniFit.Application.Validators.Workouts
{
    public class WorkoutSetRequestValidator : AbstractValidator<WorkoutSetRequest>
    {
        public WorkoutSetRequestValidator()
        {
            RuleFor(x => x.SetNumber)
                .GreaterThan(0).WithMessage("Set Number must be greater than 0");

            RuleFor(x => x.Reps)
                .GreaterThanOrEqualTo(0).WithMessage("Reps cannot be negative");

            RuleFor(x => x.Weight)
                .GreaterThanOrEqualTo(0).WithMessage("Weight cannot be negative");
        }
    }
}
