using FluentValidation;
using OmniFit.Application.DTOs.Workouts;
using OmniFit.Domain.Interfaces;

namespace OmniFit.Application.Validators.Workouts
{
    public class WorkoutExerciseRequestValidator : AbstractValidator<WorkoutExerciseRequest>
    {
        public WorkoutExerciseRequestValidator(IExerciseRepository exerciseRepository)
        {
            RuleFor(x => x.ExerciseId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Exercise ID is required")
                .MustAsync(async (exerciseId, ct) => await exerciseRepository.GetByIdAsync(exerciseId) != null)
                .WithMessage("Exercise does not exist");
        }
    }
}
