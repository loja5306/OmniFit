using FluentValidation.TestHelper;
using OmniFit.Application.DTOs;
using OmniFit.Application.Validators;

namespace OmniFit.Application.Tests.Unit.Validators
{
    public class WorkoutExerciseRequestValidatorTests
    {
        private readonly WorkoutExerciseRequestValidator _sut = new();

        [Fact]
        public void WorkoutExerciseRequestValidator_ShouldThrowError_WhenExerciseIdIsEmpty()
        {
            //Arrange
            var request = new WorkoutExerciseRequest(Guid.Empty);

            //Act
            var result = _sut.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.ExerciseId)
                .WithErrorMessage("Exercise ID is required");
        }

        [Fact]
        public void WorkoutExerciseRequestValidator_ShouldThrowError_WhenNoSetsExist()
        {
            //Arrange
            var request = new WorkoutExerciseRequest(Guid.NewGuid());

            //Act
            var result = _sut.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Sets)
                .WithErrorMessage("Each exercise must have at least 1 set");
        }
    }
}
