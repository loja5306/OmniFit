
using FluentAssertions;
using NSubstitute;
using OmniFit.Application.DTOs.Workouts;
using OmniFit.Application.Services;
using OmniFit.Domain.Common;
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
            var result = await _sut.CreateWorkoutAsync(request, Guid.NewGuid().ToString());

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
            var queryFilter = new WorkoutQueryParameters();

            _workoutRepository.GetAllAsync(queryFilter.Page, queryFilter.PageSize)
                .Returns(new PagedResult<Workout>(new List<Workout>(), 1, 20, 0));

            //Act
            var result = await _sut.GetAllWorkoutsAsync(queryFilter);

            //Assert
            result.Items.Should().BeEmpty();
            result.TotalCount.Should().Be(0);
            result.Page.Should().Be(queryFilter.Page);
            result.PageSize.Should().Be(queryFilter.PageSize);
        }

        [Fact]
        public async Task GetAllWorkoutsAsync_ShouldReturnResponse_WhenWorkoutsExist()
        {
            //Arrange
            var queryFilter = new WorkoutQueryParameters();
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

            var pagedWorkouts = new PagedResult<Workout>
                (workouts, queryFilter.Page, queryFilter.PageSize, workouts.Count);

            _workoutRepository.GetAllAsync(queryFilter.Page, queryFilter.PageSize).Returns(pagedWorkouts);

            //Act
            var result = await _sut.GetAllWorkoutsAsync(queryFilter);

            //Assert
            result.Items.Should().HaveCount(3);
            result.Items.Should().Contain(w => w.Name == "Monday Workout");
            result.Items.Should().Contain(w => w.Name == "Tuesday Workout");
            result.Items.Should().Contain(w => w.Name == "Wednesday Workout");
            result.TotalCount.Should().Be(3);
            result.Page.Should().Be(queryFilter.Page);
            result.PageSize.Should().Be(queryFilter.PageSize);
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

        [Fact]
        public async Task GetWorkoutsByUserIdAsync_ShouldReturnResponse_WhenWorkoutsExistForUser()
        {
            //Arrange
            var queryFilter = new WorkoutQueryParameters();
            var userId = Guid.NewGuid().ToString();
            var workouts = new List<Workout>
            {
                new Workout
                {
                    Id = Guid.NewGuid(),
                    Name = "Monday Workout",
                    UserId = userId
                    
                },
                new Workout
                {
                    Id = Guid.NewGuid(),
                    Name = "Tuesday Workout",
                    UserId = userId
                },
            };
            var pagedWorkouts = new PagedResult<Workout>
                (workouts, queryFilter.Page, queryFilter.PageSize, workouts.Count);

            _workoutRepository.GetByUserIdAsync(queryFilter.Page, queryFilter.PageSize, userId).Returns(pagedWorkouts);

            //Act
            var result = await _sut.GetWorkoutsByUserIdAsync(queryFilter, userId);

            //Assert
            result.Items.Should().HaveCount(2);
            result.Items.Should().Contain(w => w.Name == "Monday Workout");
            result.Items.Should().Contain(w => w.Name == "Tuesday Workout");
            result.TotalCount.Should().Be(2);
            result.Page.Should().Be(queryFilter.Page);
            result.PageSize.Should().Be(queryFilter.PageSize);
        }

        [Fact]
        public async Task GetAllWorkoutsAsync_ShouldReturnEmptyList_WhenNoWorkoutsExistForUser()
        {
            //Arrange
            var queryFilter = new WorkoutQueryParameters();
            var userId = Guid.NewGuid().ToString();
            _workoutRepository.GetByUserIdAsync(queryFilter.Page, queryFilter.PageSize, userId)
                .Returns(new PagedResult<Workout>(new List<Workout>(), 1, 20, 0));

            //Act
            var result = await _sut.GetWorkoutsByUserIdAsync(queryFilter, userId);

            //Assert
            result.Items.Should().BeEmpty();
            result.TotalCount.Should().Be(0);
            result.Page.Should().Be(queryFilter.Page);
            result.PageSize.Should().Be(queryFilter.PageSize);
        }
    }
}
