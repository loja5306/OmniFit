using FluentValidation;
using OmniFit.Application.DTOs;

namespace OmniFit.Application.Validators
{
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
}
