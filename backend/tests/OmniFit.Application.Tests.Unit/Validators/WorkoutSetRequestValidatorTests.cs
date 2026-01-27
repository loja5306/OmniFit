using FluentValidation.TestHelper;
using OmniFit.Application.DTOs;
using OmniFit.Application.Validators;

namespace OmniFit.Application.Tests.Unit.Validators
{
    public class WorkoutSetRequestValidatorTests
    {
        private readonly WorkoutSetRequestValidator _sut = new();

        [Fact]
        public void WorkoutSetRequestValidator_ShouldThrowError_WhenRepsAreNegative()
        {
            //Arrange
            var request = new WorkoutSetRequest(1, -1, 60);

            //Act
            var result = _sut.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Reps)
                .WithErrorMessage("Reps cannot be negative");
        }

        [Fact]
        public void WorkoutSetRequestValidator_ShouldThrowError_WhenWeightIsNegative()
        {
            //Arrange
            var request = new WorkoutSetRequest(1, 10, -1);

            //Act
            var result = _sut.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Weight)
                .WithErrorMessage("Weight cannot be negative");
        }
    }
}
