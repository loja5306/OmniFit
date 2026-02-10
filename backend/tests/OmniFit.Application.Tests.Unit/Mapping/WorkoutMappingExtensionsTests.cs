using FluentAssertions;
using OmniFit.Application.DTOs;
using OmniFit.Application.Mapping;
using OmniFit.Domain.Entities;

namespace OmniFit.Application.Tests.Unit.Mapping
{
    public class WorkoutMappingExtensionsTests
    {
        [Fact]
        public void MapToEntity_ShouldMapRequestToEntity_WhenRequestProvided()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString();
            var setRequest = new WorkoutSetRequest(1, 10, 60);
            var exerciseRequest = new WorkoutExerciseRequest(Guid.NewGuid(), new List<WorkoutSetRequest> { setRequest });
            var request = new CreateWorkoutRequest("Monday Workout", new List<WorkoutExerciseRequest> { exerciseRequest });

            //Act
            var result = request.MapToEntity(userId);

            //Assert
            result.Id.Should().NotBeEmpty();
            result.Name.Should().Be(request.Name);
            result.UserId.Should().Be(userId);
            result.WorkoutExercises.Should().NotBeNull();
            result.WorkoutExercises.First().WorkoutSets.Should().NotBeNull();
            result.WorkoutExercises.First().WorkoutSets.Should().Contain(e => e.SetNumber == setRequest.SetNumber);
            result.WorkoutExercises.First().WorkoutSets.Should().Contain(e => e.Reps == setRequest.Reps);
            result.WorkoutExercises.First().WorkoutSets.Should().Contain(e => e.Weight == setRequest.Weight);
        }

        [Fact]
        public void MapToEntity_ShouldMapRequestToEntity_WhenRequestProvidedWithNoExercises()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString();
            var request = new CreateWorkoutRequest("Monday Workout", null);

            //Act
            var result = request.MapToEntity(userId);

            //Assert
            result.Id.Should().NotBeEmpty();
            result.Name.Should().Be(request.Name);
            result.UserId.Should().Be(userId);
            result.WorkoutExercises.Count.Should().Be(0);
        }

        [Fact]
        public void MapToEntity_ShouldMapRequestToEntity_WhenRequestProvidedWithNoSets()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString();
            var exerciseRequest = new WorkoutExerciseRequest(Guid.NewGuid(), null);
            var request = new CreateWorkoutRequest("Monday Workout", new List<WorkoutExerciseRequest> { exerciseRequest });

            //Act
            var result = request.MapToEntity(userId);

            //Assert
            result.Id.Should().NotBeEmpty();
            result.Name.Should().Be(request.Name);
            result.UserId.Should().Be(userId);
            result.WorkoutExercises.Should().NotBeNull();
            result.WorkoutExercises.First().WorkoutSets.Count.Should().Be(0);
        }

        [Fact]
        public void MapToResponse_ShouldMapEntityToResponse_WhenEntityProvided()
        {
            //Arrange
            var workout = new Workout
            {
                Id = Guid.NewGuid(),
                Name = "Monday Workout",
                WorkoutExercises = new List<WorkoutExercise>
                {
                    new WorkoutExercise
                    {
                        Id = Guid.NewGuid(),
                        Exercise = new Exercise
                        {
                            Id = Guid.NewGuid(),
                            Name = "Bench Press",
                            Description = "Chest Exercise",
                        },
                        WorkoutSets = new List<WorkoutSet>
                        {
                            new WorkoutSet
                            {
                                Id = Guid.NewGuid(),
                                SetNumber = 1,
                                Reps = 10,
                                Weight = 60
                            }
                        }
                    }
                }
            };

            //Act
            var result = workout.MapToResponse();

            //Assert
            result.Id.Should().Be(workout.Id);
            result.Name.Should().Be(workout.Name);
            result.TotalExercises.Should().Be(workout.WorkoutExercises.Count());
        }
    }
}
