using FluentValidation.TestHelper;
using OmniFit.Application.DTOs.Exercises;
using OmniFit.Application.Validators.Exercises;

namespace OmniFit.Application.Tests.Unit.Validators
{
    public class CreateExerciseRequestValidatorTests
    {
        private readonly ExerciseRequestValidator _sut = new();

        [Fact]
        public void CreateExerciseRequestValidator_ShouldThrowError_WhenNameIsEmpty()
        {
            //Arrange
            var request = new ExerciseRequest("", "Custom Exercise");

            //Act
            var result = _sut.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("Exercise name is required");
        }

        [Fact]
        public void CreateExerciseRequestValidator_ShouldThrowError_WhenNameIsOver100Characters()
        {
            //Arrange
            var request = new ExerciseRequest(new string('A', 101), "Custom Exercise");

            //Act
            var result = _sut.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("Exercise name cannot exceed 100 characters");
        }

        [Fact]
        public void CreateExerciseRequestValidator_ShouldThrowError_WhenDescriptionIsOver500Characters()
        {
            //Arrange
            var request = new ExerciseRequest("Bench Press", new string('A', 501));

            //Act
            var result = _sut.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Description)
                .WithErrorMessage("Exercise description cannot exceed 500 characters");
        }
    }
}
