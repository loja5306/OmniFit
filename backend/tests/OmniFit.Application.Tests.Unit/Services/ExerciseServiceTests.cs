using FluentAssertions;
using NSubstitute;
using OmniFit.Application.DTOs;
using OmniFit.Application.Services;
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
            _exerciseRepository.GetAllAsync().Returns(new List<Exercise>());

            //Act
            var result = await _sut.GetAllAsync();

            //Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnResponse_WhenExercisesExist()
        {
            //Arrange
            var exercises = new List<Exercise>
            {
                new Exercise { Id = Guid.NewGuid(), Name = "Bench Press", Description = "Chest Exercise" },
                new Exercise { Id = Guid.NewGuid(), Name = "Squat", Description = "Quad Exercise" },
                new Exercise { Id = Guid.NewGuid(), Name = "Pull Up", Description = "Lats Exercise" }
            };

            _exerciseRepository.GetAllAsync().Returns(exercises);

            //Act
            var result = await _sut.GetAllAsync();

            //Assert
            result.Should().HaveCount(3);
            result.Should().Contain(e => e.Name == "Bench Press" && e.Description == "Chest Exercise");
            result.Should().Contain(e => e.Name == "Squat" && e.Description == "Quad Exercise");
            result.Should().Contain(e => e.Name == "Pull Up" && e.Description == "Lats Exercise");
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
