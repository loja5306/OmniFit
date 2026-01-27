
using FluentAssertions;
using NSubstitute;
using OmniFit.Application.DTOs;
using OmniFit.Application.Services;
using OmniFit.Domain.Entities;
using OmniFit.Domain.Interfaces;

namespace OmniFit.Application.Tests.Unit.Services
{
    public class WorkoutServiceTests
    {
        private readonly WorkoutService _sut;

        private readonly IWorkoutRepository _workoutRepository =
            Substitute.For<IWorkoutRepository>();

        public WorkoutServiceTests()
        {
            _sut = new WorkoutService(_workoutRepository);
        }

        [Fact]
        public async Task CreateWorkoutAsync_ShouldReturnIdAndCreateWorkout_WhenMethodCalled()
        {
            //Arrange
            var request = new CreateWorkoutRequest("Monday Workout");

            //Act
            var result = await _sut.CreateWorkoutAsync(request);

            //Assert
            result.Should().NotBeEmpty();

            await _workoutRepository.Received(1).AddAsync(Arg.Any<Workout>());
            await _workoutRepository.Received(1).SaveChangesAsync();
        }

        [Fact]
        public async Task CreateWorkoutAsync_ShouldMapNestedExercisesAndSets_WhenExerciseAndSetRequestsProvided()
        {
            //Arrange
            var setRequest = new WorkoutSetRequest(1, 10, 60);
            var exerciseRequest = new WorkoutExerciseRequest(Guid.NewGuid(), new List<WorkoutSetRequest> { setRequest });
            var request = new CreateWorkoutRequest("Monday Workout", new List<WorkoutExerciseRequest> { exerciseRequest });

            //Act
            var result = await _sut.CreateWorkoutAsync(request);

            await _workoutRepository.Received(1).AddAsync(Arg.Is<Workout>(w =>
                w.Name == "Monday Workout" &&
                w.WorkoutExercises.Count == 1 &&
                w.WorkoutExercises.First().ExerciseId == exerciseRequest.ExerciseId &&
                w.WorkoutExercises.First().WorkoutSets.Count == 1 &&
                w.WorkoutExercises.First().WorkoutSets.First().Reps == 10 &&
                w.WorkoutExercises.First().WorkoutSets.First().Weight == 60
            ));
        }

        [Fact]
        public async Task GetAllWorkoutsAsync_ShouldReturnEmptyList_WhenNoWorkoutsExist()
        {
            //Arrange
            _workoutRepository.GetAllAsync().Returns(new List<Workout>());

            //Act
            var result = await _sut.GetAllWorkoutsAsync();

            //Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllWorkoutsAsync_ShouldReturnMappedWorkouts_WhenWorkoutsExist()
        {
            //Arrange
            var workouts = new List<Workout>
            {
                new Workout
                {
                    Id = Guid.NewGuid(),
                    Name = "Monday Workout"
                },
                new Workout
                {
                    Id = Guid.NewGuid(),
                    Name = "Tuesday Workout"
                },
                new Workout
                {
                    Id = Guid.NewGuid(),
                    Name = "Wednesday Workout"
                }
            };

            _workoutRepository.GetAllAsync().Returns(workouts);

            //Act
            var result = await _sut.GetAllWorkoutsAsync();

            //Assert
            result.Should().HaveCount(3);
            result.Should().Contain(w => w.Name == "Monday Workout");
            result.Should().Contain(w => w.Name == "Tuesday Workout");
            result.Should().Contain(w => w.Name == "Wednesday Workout");
        }

        [Fact]
        public async Task GetWorkoutByIdAsync_ShouldReturnExercise_WhenExerciseExists()
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
            _workoutRepository.GetByIdAsync(workout.Id).Returns(workout);

            //Act
            var result = await _sut.GetWorkoutByIdAsync(workout.Id);

            //Assert
            result!.Id.Should().Be(workout.Id);
            result!.Name.Should().Be(workout.Name);
            result!.TotalExercises.Should().Be(1);
        }

        [Fact]
        public async Task GetWorkoutByIdAsync_ShouldReturnNull_WhenWorkoutDoesNotExist()
        {
            //Arrange
            var id = Guid.NewGuid();
            _workoutRepository.GetByIdAsync(id).Returns((Workout?)null);

            //Act
            var result = await _sut.GetWorkoutByIdAsync(id);

            //Assert
            result.Should().BeNull();
        }
    }
}
