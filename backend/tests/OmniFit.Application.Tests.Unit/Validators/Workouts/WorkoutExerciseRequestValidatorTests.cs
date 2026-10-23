using FluentValidation.TestHelper;
using NSubstitute;
using OmniFit.Application.DTOs.Workouts;
using OmniFit.Application.Validators.Workouts;
using OmniFit.Domain.Entities;
using OmniFit.Domain.Interfaces;

namespace OmniFit.Application.Tests.Unit.Validators
{
    public class WorkoutExerciseRequestValidatorTests
    {
        private readonly IExerciseRepository _exerciseRepository = 
            Substitute.For<IExerciseRepository>();
        private readonly WorkoutExerciseRequestValidator _sut;

        public WorkoutExerciseRequestValidatorTests()
        {
            _sut = new WorkoutExerciseRequestValidator(_exerciseRepository);
        }

        [Fact]
        public async Task WorkoutExerciseRequestValidator_ShouldNotThrowError_WhenExerciseExists()
        {
            //Arrange
            var exerciseId = Guid.NewGuid();
            var request = new WorkoutExerciseRequest(exerciseId);

            _exerciseRepository.GetByIdAsync(exerciseId).Returns(
                new Exercise
                {
                    Id = exerciseId,
                    Name = "Bench Press"
                });

            //Act
            var result = await _sut.TestValidateAsync(request);
            
            //Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task WorkoutExerciseRequestValidator_ShouldThrowError_WhenExerciseIdIsEmpty()
        {
            //Arrange
            var request = new WorkoutExerciseRequest(Guid.Empty);

            //Act
            var result = await _sut.TestValidateAsync(request);
            
            //Assert
            result.ShouldHaveValidationErrorFor(x => x.ExerciseId)
                .WithErrorMessage("Exercise ID is required");
        }

        [Fact]
        public async Task WorkoutExerciseRequestValidator_ShouldThrowError_WhenExerciseDoesNotExist()
        {
            //Arrange
            var exerciseId = Guid.NewGuid();
            var request = new WorkoutExerciseRequest(exerciseId);

            _exerciseRepository.GetByIdAsync(exerciseId).Returns((Exercise?)null);

            //Act
            var result = await _sut.TestValidateAsync(request);
            
            //Assert
            result.ShouldHaveValidationErrorFor(x => x.ExerciseId)
                .WithErrorMessage("Exercise does not exist");
        }
    }
}