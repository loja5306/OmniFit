using FluentAssertions;
using OmniFit.Application.DTOs;
using OmniFit.Application.Mapping;
using OmniFit.Domain.Entities;

namespace OmniFit.Application.Tests.Unit.Mapping
{
    public class ExerciseMappingExtensionsTests
    {
        [Fact]
        public void MapToEntity_ShouldMapRequestToEntity_WhenRequestProvided()
        {
            //Arrange
            var request = new CreateExerciseRequest("Bench Press", "Chest Exercise");

            //Act
            var result = request.MapToEntity();

            //Assert
            result.Id.Should().NotBeEmpty();
            result.Name.Should().Be(request.Name);
            result.Description.Should().Be(request.Description);
        }

        [Fact]
        public void MapToResponse_ShouldMapEntityToResponse_WhenEntityProvided()
        {
            //Arrange
            var exercise = new Exercise
            {
                Id = Guid.NewGuid(),
                Name = "Bench Press",
                Description = "Chest Exercise"
            };

            //Act
            var result = exercise.MapToResponse();

            //Assert
            result.Id.Should().Be(exercise.Id);
            result.Name.Should().Be(exercise.Name);
            result.Description.Should().Be(exercise.Description);
        }
    }
}
