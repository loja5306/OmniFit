using FluentValidation.TestHelper;
using OmniFit.Application.DTOs;
using OmniFit.Application.Validators;

namespace OmniFit.Application.Tests.Unit.Validators
{
    public class CreateWorkoutRequestValidatorTests
    {
        private readonly CreateWorkoutRequestValidator _sut = new();

        [Fact]
        public void CreateWorkoutRequestValidator_ShouldThrowError_WhenNameEmpty()
        {
            //Arrange
            var request = new CreateWorkoutRequest("");

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
            var request = new CreateWorkoutRequest(new string('A', 101));

            //Act
            var result = _sut.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("Workout name cannot exceed 100 characters");
        }

        [Fact]
        public void CreateWorkoutRequestValidator_ShouldThrowError_WhenNoExercisesExist()
        {
            //Arrange
            var request = new CreateWorkoutRequest("Monday Workout");

            //Act
            var result = _sut.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Exercises)
                .WithErrorMessage("A workout must have at least 1 exercise");
        }

        public void CreateWorkoutRequestValidator_ShouldThrowError_WhenExerciseHasNoSets()
        {
            //Arrange
            var exerciseRequest = new WorkoutExerciseRequest(Guid.NewGuid());
            var request = new CreateWorkoutRequest("Monday Workout", new List<WorkoutExerciseRequest> { exerciseRequest });

            //Act
            var result = _sut.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Exercises!.First().Sets)
                .WithErrorMessage("Each exercise must have at least 1 set");
        }
    }
}
