using FluentValidation.TestHelper;
using OmniFit.Application.DTOs.Workouts;
using OmniFit.Application.Validators.Workouts;

namespace OmniFit.Application.Tests.Unit.Validators
{
    public class CreateWorkoutRequestValidatorTests
    {
        private readonly WorkoutRequestValidator _sut = new();

        [Fact]
        public void CreateWorkoutRequestValidator_ShouldThrowError_WhenNameEmpty()
        {
            //Arrange
            var request = new WorkoutRequest("");

            //Act
            var result = _sut.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("Workout name is required");
        }

        [Fact]
        public void CreateWorkoutRequestValidator_ShouldThrowError_WhenNameIsOver100Characters()
        {
            //Arrange
            var request = new WorkoutRequest(new string('A', 101));

            //Act
            var result = _sut.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("Workout name cannot exceed 100 characters");
        }
    }
}
