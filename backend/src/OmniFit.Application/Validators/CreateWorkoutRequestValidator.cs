using FluentValidation;
using OmniFit.Application.DTOs;

namespace OmniFit.Application.Validators
{
    public class CreateWorkoutRequestValidator : AbstractValidator<CreateWorkoutRequest>
    {
        public CreateWorkoutRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Workout name is required")
                .MaximumLength(100).WithMessage("Workout name cannot exceed 100 characters");

            RuleFor(x => x.Exercises)
                .NotEmpty().WithMessage("A workout must have at least 1 exercise");

            RuleForEach(x => x.Exercises)
                .SetValidator(new WorkoutExerciseRequestValidator());
        }
    }
}
