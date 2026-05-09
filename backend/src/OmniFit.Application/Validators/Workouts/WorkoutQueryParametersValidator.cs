using FluentValidation;
using OmniFit.Application.DTOs.Workouts;

namespace OmniFit.Application.Validators.Workouts
{
    public class WorkoutQueryParametersValidator : AbstractValidator<WorkoutQueryParameters>
    {
        public WorkoutQueryParametersValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(1).WithMessage("Page must be greater than or equal to 1");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100");
        }
    }
}
