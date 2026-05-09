using FluentAssertions;
using NSubstitute;
using OmniFit.Application.DTOs.Exercises;
using OmniFit.Application.Services;
using OmniFit.Domain.Common;
using OmniFit.Domain.Entities;
using OmniFit.Domain.Interfaces;

namespace OmniFit.Application.Tests.Unit.Services
{
    public class ExerciseServiceTests
    {
        private readonly ExerciseService _sut;

        private readonly IExerciseRepository _exerciseRepository = 
            Substitute.For<IExerciseRepository>();

        public ExerciseServiceTests()
        {
            _sut = new ExerciseService(_exerciseRepository);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateExerciseAndReturnId_WhenMethodCalled()
        {
            //Arrange
            var request = new CreateExerciseRequest("Bench Press", "Chest Exercise");

            //Act
            var result = await _sut.CreateAsync(request);

            //Assert
            result.Should().NotBeEmpty();
            
            await _exerciseRepository.Received(1).AddAsync(Arg.Any<Exercise>());
            await _exerciseRepository.Received(1).SaveChangesAsync();
        }

        [Fact]
        public async Task DeleteByIdAsync_ShouldDeleteExerciseAndReturnTrue_WhenExerciseExists()
        {
            //Arrange
            var exercise = new Exercise
            {
                Id = Guid.NewGuid(),
                Name = "Bench Press"
            };

            _exerciseRepository.GetByIdAsync(exercise.Id).Returns(exercise);

            //Act
            var result = await _sut.DeleteByIdAsync(exercise.Id);

            //Assert
            result.Should().Be(true);
            
            _exerciseRepository.Received(1).Delete(exercise);
            await _exerciseRepository.Received(1).SaveChangesAsync();

        }

        [Fact]
        public async Task DeleteByIdAsync_ShouldReturnFalse_WhenExerciseDoesNotExist()
        {
            //Arrange
            var id = Guid.NewGuid();
            _exerciseRepository.GetByIdAsync(id).Returns((Exercise?)null);

            //Act
            var result = await _sut.DeleteByIdAsync(id);

            //Assert
            result.Should().Be(false);

            _exerciseRepository.DidNotReceive().Delete(Arg.Any<Exercise>());
            await _exerciseRepository.DidNotReceive().SaveChangesAsync();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoExercisesExist()
        {
            //Arrange
            var queryFilter = new ExerciseQueryParameters();
            _exerciseRepository.GetAllAsync(queryFilter.Page, queryFilter.PageSize)
                .Returns(new PagedResult<Exercise>(new List<Exercise>(), 1, 20, 0));

            //Act
            var result = await _sut.GetAllAsync(queryFilter);

            //Assert
            result.Items.Should().BeEmpty();
            result.TotalCount.Should().Be(0);
            result.Page.Should().Be(queryFilter.Page);
            result.PageSize.Should().Be(queryFilter.PageSize);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnResponse_WhenExercisesExist()
        {
            //Arrange
            var queryFilter = new ExerciseQueryParameters();
            var exercises = new List<Exercise>
            {
                new Exercise { Id = Guid.NewGuid(), Name = "Bench Press", Description = "Chest Exercise" },
                new Exercise { Id = Guid.NewGuid(), Name = "Squat", Description = "Quad Exercise" },
                new Exercise { Id = Guid.NewGuid(), Name = "Pull Up", Description = "Lats Exercise" }
            };

            var pagedExercises = new PagedResult<Exercise>
                (exercises, queryFilter.Page, queryFilter.PageSize, exercises.Count);

            _exerciseRepository.GetAllAsync(queryFilter.Page, queryFilter.PageSize).Returns(pagedExercises);

            //Act
            var result = await _sut.GetAllAsync(queryFilter);

            //Assert
            result.Items.Should().HaveCount(3);
            result.Items.Should().Contain(e => e.Name == "Bench Press" && e.Description == "Chest Exercise");
            result.Items.Should().Contain(e => e.Name == "Squat" && e.Description == "Quad Exercise");
            result.Items.Should().Contain(e => e.Name == "Pull Up" && e.Description == "Lats Exercise");
            result.TotalCount.Should().Be(3);
            result.Page.Should().Be(queryFilter.Page);
            result.PageSize.Should().Be(queryFilter.PageSize);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnExercise_WhenExerciseExists()
        {
            //Arrange
            var exercise = new Exercise
            {
                Id = Guid.NewGuid(),
                Name = "Bench Press"
            };
            _exerciseRepository.GetByIdAsync(exercise.Id).Returns(exercise);

            //Act
            var result = await _sut.GetByIdAsync(exercise.Id);

            //Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(exercise.Id);
            result!.Name.Should().Be(exercise.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenExerciseDoesNotExist()
        {
            //Arrange
            var id = Guid.NewGuid();
            _exerciseRepository.GetByIdAsync(id).Returns((Exercise?)null);

            //Act
            var result = await _sut.GetByIdAsync(id);

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNull_WhenExerciseDoesNotExist()
        {
            //Arrange
            var id = Guid.NewGuid();
            var updateRequest = new UpdateExerciseRequest("Bench Press", "Chest Exercise");
            _exerciseRepository.GetByIdAsync(id).Returns((Exercise?)null);

            //Act
            var result = await _sut.UpdateAsync(id, updateRequest);

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task UpdateAsync_ShouldCallRespository_WhenExerciseExists()
        {
            //Arrange
            var id = Guid.NewGuid();
            var updateRequest = new UpdateExerciseRequest("Bench Press", "Chest Exercise");

            var existingExercise = new Exercise
            {
                Id = id,
                Name = "Squat",
                Description = "Quad Exercise"
            };

            _exerciseRepository.GetByIdAsync(id).Returns(existingExercise);

            //Act
            var result = await _sut.UpdateAsync(id, updateRequest);

            //Assert
            _exerciseRepository.Received(1).Update(Arg.Any<Exercise>());
            await _exerciseRepository.Received(1).SaveChangesAsync();
        }
    }
}
