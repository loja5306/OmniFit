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

    public class WorkoutExerciseRequestValidator : AbstractValidator<WorkoutExerciseRequest>
    {
        public WorkoutExerciseRequestValidator()
        {
            RuleFor(x => x.ExerciseId)
                .NotEmpty().WithMessage("Exercise ID is required");

            RuleFor(x => x.Sets)
                .NotEmpty().WithMessage("Each exercise must have at least 1 set");

            RuleForEach(x => x.Sets)
                .SetValidator(new WorkoutSetRequestValidator());
        }
    }

    public class WorkoutSetRequestValidator : AbstractValidator<WorkoutSetRequest>
    {
        public WorkoutSetRequestValidator()
        {
            RuleFor(x => x.Reps)
                .GreaterThan(0).WithMessage("Reps cannot be negative");

            RuleFor(x => x.Weight)
                .GreaterThanOrEqualTo(0).WithMessage("Weight cannot be negative");
        }
    }
}
