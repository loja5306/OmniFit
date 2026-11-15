using FluentValidation;
using OmniFit.Application.DTOs.Workouts;

namespace OmniFit.Application.Validators.Workouts
{
    public class WorkoutRequestValidator : AbstractValidator<WorkoutRequest>
    {
        public WorkoutRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Workout name is required")
                .MaximumLength(100).WithMessage("Workout name cannot exceed 100 characters");
        }
    }
}
