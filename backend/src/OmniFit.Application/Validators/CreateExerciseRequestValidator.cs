using FluentValidation;
using OmniFit.Application.DTOs;

namespace OmniFit.Application.Validators
{
    public class CreateExerciseRequestValidator : AbstractValidator<CreateExerciseRequest>
    {
        public CreateExerciseRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Exercise name is required")
                .MaximumLength(100).WithMessage("Exercise name cannot exceed 100 characters");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Exercise description cannot exceed 500 characters");
        }
    }
}
