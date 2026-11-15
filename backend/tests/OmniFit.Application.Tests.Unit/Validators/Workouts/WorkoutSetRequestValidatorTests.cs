using FluentValidation.TestHelper;
using OmniFit.Application.DTOs.Workouts;
using OmniFit.Application.Validators.Workouts;

namespace OmniFit.Application.Tests.Unit.Validators
{
    public class WorkoutSetRequestValidatorTests
    {
        private readonly WorkoutSetRequestValidator _sut = new();

        [Fact]
        public void WorkoutSetRequestValidator_ShouldThrowError_WhenSetNumberIsInvalid()
        {
            //Arrange
            var request = new WorkoutSetRequest(0, 10, 100);

            //Act
            var result = _sut.TestValidate(request);
            
            //Assert
            result.ShouldHaveValidationErrorFor(x => x.SetNumber)
                .WithErrorMessage("Set Number must be greater than 0");
        }

        [Fact]
        public void WorkoutSetRequestValidator_ShouldThrowError_WhenRepsIsInvalid()
        {
            //Arrange
            var request = new WorkoutSetRequest(1, -10, 100);

            //Act
            var result = _sut.TestValidate(request);
            
            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Reps)
                .WithErrorMessage("Reps cannot be negative");
        }

        [Fact]
        public void WorkoutSetRequestValidator_ShouldThrowError_WhenWeightIsInvalid()
        {
            //Arrange
            var request = new WorkoutSetRequest(1, 10, -100);

            //Act
            var result = _sut.TestValidate(request);
            
            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Weight)
                .WithErrorMessage("Weight cannot be negative");
        }
    }
}