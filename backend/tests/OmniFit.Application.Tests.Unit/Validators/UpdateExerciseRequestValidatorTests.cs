using FluentValidation.TestHelper;
using OmniFit.Application.DTOs;
using OmniFit.Application.Validators;

namespace OmniFit.Application.Tests.Unit.Validators
{
    public class UpdateExerciseRequestValidatorTests
    {
        private readonly UpdateExerciseRequestValidator _sut = new();

        [Fact]
        public void UpdateExerciseRequestValidator_ShouldThrowError_WhenNameIsEmpty()
        {
            //Arrange
            var request = new UpdateExerciseRequest("", "Custom Exercise");

            //Act
            var result = _sut.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("Exercise name is required");
        }

        [Fact]
        public void UpdateExerciseRequestValidator_ShouldThrowError_WhenNameIsOver100Characters()
        {
            //Arrange
            var request = new UpdateExerciseRequest(new string('A', 101), "Custom Exercise");

            //Act
            var result = _sut.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("Exercise name cannot exceed 100 characters");
        }

        [Fact]
        public void UpdateExerciseRequestValidator_ShouldThrowError_WhenDescriptionIsOver500Characters()
        {
            //Arrange
            var request = new UpdateExerciseRequest("Bench Press", new string('A', 501));

            //Act
            var result = _sut.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Description)
                .WithErrorMessage("Exercise description cannot exceed 500 characters");
        }
    }
}
