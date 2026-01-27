
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
        public async Task CreateWorkoutAsync_ShouldReturnIdAndCallRepository_WhenWorkoutProvided()
        {
            //Arrange
            var request = new CreateWorkoutRequest("Monday Workout");

            //Act
            var result = await _sut.CreateWorkoutAsync(request);

            //Assert
            result.Should().NotBeEmpty();

            await _workoutRepository.Received(1).AddAsync(
                Arg.Is<Workout>(w => w.Name == request.Name));
            await _workoutRepository.Received(1).SaveChangesAsync();
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
        public async Task GetAllWorkoutsAsync_ShouldReturnResponse_WhenWorkoutsExist()
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
                Name = "Monday Workout"
            };
            _workoutRepository.GetByIdAsync(workout.Id).Returns(workout);

            //Act
            var result = await _sut.GetWorkoutByIdAsync(workout.Id);

            //Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(workout.Id);
            result!.Name.Should().Be(workout.Name);
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
