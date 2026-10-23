
using FluentAssertions;
using NSubstitute;
using OmniFit.Application.DTOs.Workouts;
using OmniFit.Application.Services;
using OmniFit.Domain.Common;
using OmniFit.Domain.Entities;
using OmniFit.Domain.Enums;
using OmniFit.Domain.Exceptions;
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
            var request = new WorkoutRequest("Monday Workout");
            var userId = Guid.NewGuid().ToString();

            //Act
            var result = await _sut.CreateWorkoutAsync(request, userId);

            //Assert
            await _workoutRepository.Received(1).AddAsync(
                Arg.Is<Workout>(
                    w => w.Id == result &&
                    w.Name == request.Name &&
                    w.UserId == userId &&
                    w.Status == WorkoutStatus.InProgress));
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
            var userId = Guid.NewGuid().ToString();
            var workouts = new List<Workout>
            {
                CreateWorkout(userId, "Monday Workout"),
                CreateWorkout(userId, "Tuesday Workout"),
                CreateWorkout(userId, "Wednesday Workout")
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
            var workout = CreateWorkout(Guid.NewGuid().ToString());
            _workoutRepository.GetByIdAsync(workout.Id).Returns(workout);

            //Act
            var result = await _sut.GetWorkoutByIdAsync(workout.Id);

            //Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(workout.Id);
            result.Name.Should().Be(workout.Name);
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
        public async Task GetWorkoutByIdAsync_ShouldReturnWorkout_WhenWorkoutExistsForUser()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString();
            var workout = CreateWorkout(userId);
            _workoutRepository.GetByIdAsync(workout.Id, userId).Returns(workout);

            //Act
            var result = await _sut.GetWorkoutByIdAsync(workout.Id, userId);

            //Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(workout.Id);
            result.Name.Should().Be(workout.Name);
        }

        [Fact]
        public async Task GetWorkoutByIdAsync_ShouldReturnNull_WhenWorkoutDoesNotExistForUser()
        {
            //Arrange
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid().ToString();
            _workoutRepository.GetByIdAsync(id, userId).Returns((Workout?)null);

            //Act
            var result = await _sut.GetWorkoutByIdAsync(id, userId);

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
                CreateWorkout(userId, "Monday Workout"),
                CreateWorkout(userId, "Tuesday Workout")
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
        public async Task GetWorkoutsByUserIdAsync_ShouldReturnEmptyList_WhenNoWorkoutsExistForUser()
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

        [Fact]
        public async Task CompleteWorkoutAsync_ShouldReturnTrueAndCallRepository_WhenRequestIsValid()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString();
            var workout = CreateWorkout(userId, exerciseCount: 1, setsPerExercise: 2);

            _workoutRepository.GetByIdAsync(workout.Id, userId).Returns(workout);
            
            //Act
            var result = await _sut.CompleteWorkoutAsync(workout.Id, userId);

            //Assert
            result.Should().BeTrue();
            workout.Status.Should().Be(WorkoutStatus.Completed);
            workout.CompletedOn.Should().NotBeNull();
            await _workoutRepository.Received(1).SaveChangesAsync();
        }

        [Fact]
        public async Task CompleteWorkoutAsync_ShouldReturnFalse_WhenWorkoutDoesNotExist()
        {
            //Arrange
            var workoutId = Guid.NewGuid();
            var userId = Guid.NewGuid().ToString();

            _workoutRepository.GetByIdAsync(workoutId, userId).Returns((Workout?)null);
            
            //Act
            var result = await _sut.CompleteWorkoutAsync(workoutId, userId);

            //Assert
            result.Should().BeFalse();
            await _workoutRepository.DidNotReceive().SaveChangesAsync();
        }
        
        [Fact]
        public async Task CompleteWorkoutAsync_ShouldThrowError_WhenWorkoutIsComplete()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString();
            var workout = CreateWorkout(userId, status: WorkoutStatus.Completed);

            _workoutRepository.GetByIdAsync(workout.Id, userId).Returns(workout);
            
            //Act
            Func<Task> act = async () => await _sut.CompleteWorkoutAsync(workout.Id, userId);

            //Assert
            await act.Should().ThrowAsync<BusinessRuleException>()
                .WithMessage("Cannot modify a completed workout");
            await _workoutRepository.DidNotReceive().SaveChangesAsync();

        }

        [Fact]
        public async Task CompleteWorkoutAsync_ShouldThrowError_WhenWorkoutHasNoSets()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString();
            var workout = CreateWorkout(userId, setsPerExercise: 0);

            _workoutRepository.GetByIdAsync(workout.Id, userId).Returns(workout);
            
            //Act
            Func<Task> act = async () => await _sut.CompleteWorkoutAsync(workout.Id, userId);

            //Assert
            await act.Should().ThrowAsync<BusinessRuleException>()
                .WithMessage("Each exercise must have at least 1 set");
            await _workoutRepository.DidNotReceive().SaveChangesAsync();
        }

        [Fact]
        public async Task CompleteWorkoutAsync_ShouldThrowError_WhenWorkoutHasNoExercises()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString();
            var workout = CreateWorkout(userId, exerciseCount: 0);

            _workoutRepository.GetByIdAsync(workout.Id, userId).Returns(workout);
            
            //Act
            Func<Task> act = async () => await _sut.CompleteWorkoutAsync(workout.Id, userId);

            //Assert
            await act.Should().ThrowAsync<BusinessRuleException>()
                .WithMessage("A workout must have at least 1 exercise");
            await _workoutRepository.DidNotReceive().SaveChangesAsync();
        }

        [Fact]
        public async Task AddExerciseAsync_ShouldReturnIdAndCallRepository_WhenRequestIsValid()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString();
            var exerciseId = Guid.NewGuid();
            var workout = CreateWorkout(userId);

            var request = new WorkoutExerciseRequest(exerciseId);
            _workoutRepository.GetByIdAsync(workout.Id, userId).Returns(workout);

            //Act
            var result = await _sut.AddExerciseAsync(workout.Id, request, userId);

            //Assert
            await _workoutRepository.Received(1).AddExerciseAsync(
                Arg.Is<WorkoutExercise>(
                    we => we.Id == result &&
                    we.ExerciseId == exerciseId && 
                    we.WorkoutId == workout.Id));
            await _workoutRepository.Received(1).SaveChangesAsync();
        }

        [Fact]
        public async Task AddExerciseAsync_ShouldReturnNull_WhenWorkoutDoesNotExist()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString();
            var exerciseId = Guid.NewGuid();
            var workoutId = Guid.NewGuid();

            var request = new WorkoutExerciseRequest(exerciseId);
            _workoutRepository.GetByIdAsync(workoutId, userId).Returns((Workout?)null);

            //Act
            var result = await _sut.AddExerciseAsync(workoutId, request, userId);

            //Assert
            result.Should().BeNull();
            await _workoutRepository.DidNotReceive().AddExerciseAsync(Arg.Any<WorkoutExercise>());
            await _workoutRepository.DidNotReceive().SaveChangesAsync();
        }

        [Fact]
        public async Task AddExerciseAsync_ShouldThrowError_WhenWorkoutIsCompleted()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString();
            var exerciseId = Guid.NewGuid();
            var workout = CreateWorkout(userId, status: WorkoutStatus.Completed);

            var request = new WorkoutExerciseRequest(exerciseId);
            _workoutRepository.GetByIdAsync(workout.Id, userId).Returns(workout);

            //Act
            Func<Task> act = async () => await _sut.AddExerciseAsync(workout.Id, request, userId);

            //Assert
            await act.Should().ThrowAsync<BusinessRuleException>()
                .WithMessage("Cannot modify a completed workout");
            await _workoutRepository.DidNotReceive().AddExerciseAsync(Arg.Any<WorkoutExercise>());
            await _workoutRepository.DidNotReceive().SaveChangesAsync();
        }

        [Fact]
        public async Task RemoveExerciseAsync_ShouldReturnTrueAndCallRepository_WhenRequestIsValid()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString();
            var workout = CreateWorkout(userId);
            var workoutExercise = workout.WorkoutExercises[0];

            _workoutRepository.GetByIdAsync(workout.Id, userId).Returns(workout);

            //Act
            var result = await _sut.RemoveExerciseAsync(workout.Id, workoutExercise.Id, userId);
            
            //Assert
            result.Should().BeTrue();
            _workoutRepository.Received(1).RemoveExercise(workoutExercise);
            await _workoutRepository.Received(1).SaveChangesAsync();
        }

        [Fact]
        public async Task RemoveExerciseAsync_ShouldReturnFalse_WhenWorkoutDoesNotExist()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString();
            var workoutId = Guid.NewGuid();
            var workoutExerciseId = Guid.NewGuid();

            _workoutRepository.GetByIdAsync(workoutId, userId).Returns((Workout?)null);

            //Act
            var result = await _sut.RemoveExerciseAsync(workoutId, workoutExerciseId, userId);
            
            //Assert
            result.Should().BeFalse();
            _workoutRepository.DidNotReceive().RemoveExercise(Arg.Any<WorkoutExercise>());
            await _workoutRepository.DidNotReceive().SaveChangesAsync();
        }

        [Fact]
        public async Task RemoveExerciseAsync_ShouldReturnFalse_WhenWorkoutExerciseDoesNotExist()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString();
            var workout = CreateWorkout(userId);
            var workoutExerciseId = Guid.NewGuid();

            _workoutRepository.GetByIdAsync(workout.Id, userId).Returns(workout);

            //Act
            var result = await _sut.RemoveExerciseAsync(workout.Id, workoutExerciseId, userId);
            
            //Assert
            result.Should().BeFalse();
            _workoutRepository.DidNotReceive().RemoveExercise(Arg.Any<WorkoutExercise>());
            await _workoutRepository.DidNotReceive().SaveChangesAsync();
        }

        [Fact]
        public async Task RemoveExerciseAsync_ShouldThrowError_WhenWorkoutIsCompleted()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString();
            var workout = CreateWorkout(userId, status: WorkoutStatus.Completed);
            var workoutExercise = workout.WorkoutExercises[0];

            _workoutRepository.GetByIdAsync(workout.Id, userId).Returns(workout);

            //Act
            Func<Task> act = async () => await _sut.RemoveExerciseAsync(workout.Id, workoutExercise.Id, userId);
            
            //Assert
            await act.Should().ThrowAsync<BusinessRuleException>()
                .WithMessage("Cannot modify a completed workout");
            _workoutRepository.DidNotReceive().RemoveExercise(Arg.Any<WorkoutExercise>());
            await _workoutRepository.DidNotReceive().SaveChangesAsync();
        }

        [Fact]
        public async Task AddSetAsync_ShouldReturnIdAndCallRepository_WhenRequestIsValid()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString();
            var workout = CreateWorkout(userId);
            var workoutExercise = workout.WorkoutExercises[0];

            var request = new WorkoutSetRequest(2, 9, 100);

            _workoutRepository.GetByIdAsync(workout.Id, userId).Returns(workout);

            //Act
            var result = await _sut.AddSetAsync(workout.Id, workoutExercise.Id, request, userId);
            
            //Assert
            await _workoutRepository.Received(1).AddSetAsync(Arg.Is<WorkoutSet>(
                ws => ws.Id == result &&
                ws.WorkoutExerciseId == workoutExercise.Id &&
                ws.SetNumber == request.SetNumber &&
                ws.Reps == request.Reps &&
                ws.Weight == request.Weight
            ));
            await _workoutRepository.Received(1).SaveChangesAsync();
        }

        [Fact]
        public async Task AddSetAsync_ShouldReturnNull_WhenWorkoutDoesNotExist()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString();
            var workoutId = Guid.NewGuid();
            var workoutExerciseId = Guid.NewGuid();

            var request = new WorkoutSetRequest(2, 9, 100);

            _workoutRepository.GetByIdAsync(workoutId, userId).Returns((Workout?)null);

            //Act
            var result = await _sut.AddSetAsync(workoutId, workoutExerciseId, request, userId);
            
            //Assert
            result.Should().BeNull();
            await _workoutRepository.DidNotReceive().AddSetAsync(Arg.Any<WorkoutSet>());
            await _workoutRepository.DidNotReceive().SaveChangesAsync();
        }

        [Fact]
        public async Task AddSetAsync_ShouldThrowError_WhenWorkoutIsCompleted()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString();
            var workout = CreateWorkout(userId, status: WorkoutStatus.Completed);
            var workoutExercise = workout.WorkoutExercises[0];

            var request = new WorkoutSetRequest(2, 9, 100);

            _workoutRepository.GetByIdAsync(workout.Id, userId).Returns(workout);

            //Act
            Func<Task> act = async () => await _sut.AddSetAsync(workout.Id, workoutExercise.Id, request, userId);
            
            //Assert
            await act.Should().ThrowAsync<BusinessRuleException>()
                .WithMessage("Cannot modify a completed workout");
            await _workoutRepository.DidNotReceive().AddSetAsync(Arg.Any<WorkoutSet>());
            await _workoutRepository.DidNotReceive().SaveChangesAsync();
        }

        [Fact]
        public async Task AddSetAsync_ShouldReturnNull_WhenWorkoutExerciseDoesNotExist()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString();
            var workout = CreateWorkout(userId);
            var workoutExerciseId = Guid.NewGuid();

            var request = new WorkoutSetRequest(2, 9, 100);

            _workoutRepository.GetByIdAsync(workout.Id, userId).Returns(workout);

            //Act
            var result = await _sut.AddSetAsync(workout.Id, workoutExerciseId, request, userId);
            
            //Assert
            result.Should().BeNull();
            await _workoutRepository.DidNotReceive().AddSetAsync(Arg.Any<WorkoutSet>());
            await _workoutRepository.DidNotReceive().SaveChangesAsync();
        }

        [Fact]
        public async Task UpdateSetAsync_ShouldReturnCorrectResponseAndCallRepository_WhenRequestIsValid()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString();
            var workout = CreateWorkout(userId);
            var workoutExercise = workout.WorkoutExercises[0];
            var workoutSet = workout.WorkoutExercises[0].WorkoutSets[0];

            var request = new WorkoutSetRequest(1, 11, 110);

            _workoutRepository.GetByIdAsync(workout.Id, userId).Returns(workout);

            //Act
            var result = await _sut.UpdateSetAsync(
                workout.Id, workoutExercise.Id, workoutSet.Id, request, userId
            );

            //Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(workoutSet.Id);
            result.SetNumber.Should().Be(request.SetNumber);
            result.Reps.Should().Be(request.Reps);
            result.Weight.Should().Be(request.Weight);
            result.WorkoutExerciseId.Should().Be(workoutExercise.Id);
            _workoutRepository.Received(1).UpdateSet(Arg.Is<WorkoutSet>(ws => 
                ws.Id == workoutSet.Id &&
                ws.SetNumber == workoutSet.SetNumber &&
                ws.Reps == ws.Reps &&
                ws.Weight == ws.Weight
            ));
            await _workoutRepository.Received(1).SaveChangesAsync();
        }

        [Fact]
        public async Task UpdateSetAsync_ShouldReturnNull_WhenWorkoutDoesNotExist()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString();
            var workoutId = Guid.NewGuid();
            var workoutExerciseId = Guid.NewGuid();
            var workoutSetId = Guid.NewGuid();

            var request = new WorkoutSetRequest(1, 11, 110);

            _workoutRepository.GetByIdAsync(workoutId, userId).Returns((Workout?)null);

            //Act
            var result = await _sut.UpdateSetAsync(
                workoutId, workoutExerciseId, workoutSetId, request, userId
            );

            //Assert
            result.Should().BeNull();
            _workoutRepository.DidNotReceive().UpdateSet(Arg.Any<WorkoutSet>());
            await _workoutRepository.DidNotReceive().SaveChangesAsync();
        }

        [Fact]
        public async Task UpdateSetAsync_ShouldReturnNull_WhenWorkoutExerciseDoesNotExist()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString();
            var workout = CreateWorkout(userId);
            var workoutExerciseId = Guid.NewGuid();
            var workoutSetId = Guid.NewGuid();

            var request = new WorkoutSetRequest(1, 11, 110);

            _workoutRepository.GetByIdAsync(workout.Id, userId).Returns(workout);

            //Act
            var result = await _sut.UpdateSetAsync(
                workout.Id, workoutExerciseId, workoutSetId, request, userId
            );

            //Assert
            result.Should().BeNull();
            _workoutRepository.DidNotReceive().UpdateSet(Arg.Any<WorkoutSet>());
            await _workoutRepository.DidNotReceive().SaveChangesAsync();
        }

        [Fact]
        public async Task UpdateSetAsync_ShouldReturnNull_WhenWorkoutSetDoesNotExist()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString();
            var workout = CreateWorkout(userId);
            var workoutExercise = workout.WorkoutExercises[0];
            var workoutSetId = Guid.NewGuid();

            var request = new WorkoutSetRequest(1, 11, 110);

            _workoutRepository.GetByIdAsync(workout.Id, userId).Returns(workout);

            //Act
            var result = await _sut.UpdateSetAsync(
                workout.Id, workoutExercise.Id, workoutSetId, request, userId
            );

            //Assert
            result.Should().BeNull();
            _workoutRepository.DidNotReceive().UpdateSet(Arg.Any<WorkoutSet>());
            await _workoutRepository.DidNotReceive().SaveChangesAsync();
        }

        [Fact]
        public async Task UpdateSetAsync_ShouldThrowError_WhenWorkoutIsCompleted()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString();
            var workout = CreateWorkout(userId, status: WorkoutStatus.Completed);
            var workoutExercise = workout.WorkoutExercises[0];
            var workoutSet = workoutExercise.WorkoutSets[0];

            var request = new WorkoutSetRequest(1, 11, 110);

            _workoutRepository.GetByIdAsync(workout.Id, userId).Returns(workout);

            //Act
            Func<Task> act = async () => await _sut.UpdateSetAsync(
                workout.Id, workoutExercise.Id, workoutSet.Id, request, userId
            );

            //Assert
            await act.Should().ThrowAsync<BusinessRuleException>()
                .WithMessage("Cannot modify a completed workout");
            _workoutRepository.DidNotReceive().UpdateSet(Arg.Any<WorkoutSet>());
            await _workoutRepository.DidNotReceive().SaveChangesAsync();
        }

        [Fact]
        public async Task RemoveSetAsync_ShouldReturnTrueAndCallRepository_WhenRequestIsValid()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString();
            var workout = CreateWorkout(userId);
            var workoutExercise = workout.WorkoutExercises[0];
            var workoutSet = workoutExercise.WorkoutSets[0];

            _workoutRepository.GetByIdAsync(workout.Id, userId).Returns(workout);

            //Act
            var result = await _sut.RemoveSetAsync(
                workout.Id, workoutExercise.Id, workoutSet.Id, userId
            );

            //Assert
            result.Should().BeTrue();
            _workoutRepository.Received(1).RemoveSet(Arg.Is<WorkoutSet>(ws => 
                ws.Id == workoutSet.Id &&
                ws.SetNumber == workoutSet.SetNumber &&
                ws.Reps == ws.Reps &&
                ws.Weight == ws.Weight
            ));
            await _workoutRepository.Received(1).SaveChangesAsync();
        }

        [Fact]
        public async Task RemoveSetAsync_ShouldReturnFalse_WhenWorkoutDoesNotExist()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString();
            var workoutId = Guid.NewGuid();
            var workoutExerciseId = Guid.NewGuid();
            var workoutSetId = Guid.NewGuid();

            _workoutRepository.GetByIdAsync(workoutId, userId).Returns((Workout?)null);

            //Act
            var result = await _sut.RemoveSetAsync(
                workoutId, workoutExerciseId, workoutSetId, userId
            );

            //Assert
            result.Should().BeFalse();
            _workoutRepository.DidNotReceive().RemoveSet(Arg.Any<WorkoutSet>());
            await _workoutRepository.DidNotReceive().SaveChangesAsync();
        }

        [Fact]
        public async Task RemoveSetAsync_ShouldReturnFalse_WhenWorkoutExerciseDoesNotExist()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString();
            var workout = CreateWorkout(userId);
            var workoutExerciseId = Guid.NewGuid();
            var workoutSetId = Guid.NewGuid();

            _workoutRepository.GetByIdAsync(workout.Id, userId).Returns(workout);

            //Act
            var result = await _sut.RemoveSetAsync(
                workout.Id, workoutExerciseId, workoutSetId, userId
            );

            //Assert
            result.Should().BeFalse();
            _workoutRepository.DidNotReceive().RemoveSet(Arg.Any<WorkoutSet>());
            await _workoutRepository.DidNotReceive().SaveChangesAsync();
        }

        [Fact]
        public async Task RemoveSetAsync_ShouldReturnFalse_WhenWorkoutSetDoesNotExist()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString();
            var workout = CreateWorkout(userId);
            var workoutExercise = workout.WorkoutExercises[0];
            var workoutSetId = Guid.NewGuid();

            _workoutRepository.GetByIdAsync(workout.Id, userId).Returns(workout);

            //Act
            var result = await _sut.RemoveSetAsync(
                workout.Id, workoutExercise.Id, workoutSetId, userId
            );

            //Assert
            result.Should().BeFalse();
            _workoutRepository.DidNotReceive().RemoveSet(Arg.Any<WorkoutSet>());
            await _workoutRepository.DidNotReceive().SaveChangesAsync();
        }

        [Fact]
        public async Task RemoveSetAsync_ShouldThrowError_WhenWorkoutIsCompleted()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString();
            var workout = CreateWorkout(userId, status: WorkoutStatus.Completed);
            var workoutExercise = workout.WorkoutExercises[0];
            var workoutSetId = Guid.NewGuid();

            _workoutRepository.GetByIdAsync(workout.Id, userId).Returns(workout);

            //Act
            Func<Task> act = async () => await _sut.RemoveSetAsync(
                workout.Id, workoutExercise.Id, workoutSetId, userId
            );

            //Assert
            await act.Should().ThrowAsync<BusinessRuleException>()
                .WithMessage("Cannot modify a completed workout");
            _workoutRepository.DidNotReceive().RemoveSet(Arg.Any<WorkoutSet>());
            await _workoutRepository.DidNotReceive().SaveChangesAsync();
        }

        #region Helpers

        private Workout CreateWorkout(
            string userId,
            string name = "Monday Workout",
            WorkoutStatus status = WorkoutStatus.InProgress,
            int exerciseCount = 1,
            int setsPerExercise = 1
        )
        {
            var workoutId = Guid.NewGuid();
            return new Workout
            {
                Id = workoutId,
                Name = name,
                UserId = userId,
                Status = status,
                WorkoutExercises = Enumerable.Range(1, exerciseCount)
                    .Select(_ => 
                    {
                        var workoutExerciseId = Guid.NewGuid();
                        return new WorkoutExercise
                        {
                            Id = workoutExerciseId,
                            ExerciseId = Guid.NewGuid(),
                            WorkoutId = workoutId,
                            WorkoutSets = Enumerable.Range(1, setsPerExercise)
                                .Select(n => new WorkoutSet
                                {
                                    Id = Guid.NewGuid(),
                                    SetNumber = n,
                                    Reps = 10,
                                    Weight = 100,
                                    WorkoutExerciseId = workoutExerciseId
                                }).ToList()
                        };
                    }).ToList()
            };
        }

        #endregion
    }
}
