using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using OmniFit.Application.DTOs;
using System.Net;
using System.Net.Http.Json;

namespace OmniFit.Api.Tests.Integration.Controllers
{
    public class ExercisesControllerTests : IClassFixture<ApiFactory>, IAsyncLifetime
    {
        private readonly ApiFactory _factory;
        private readonly HttpClient _httpClient;

        public ExercisesControllerTests(ApiFactory factory)
        {
            _factory = factory;
            _httpClient = factory.HttpClient;
        }

        [Fact]
        public async Task Create_CreatesExercise_WhenExerciseIsValid()
        {
            //Arrange
            var exercise = new CreateExerciseRequest("Bench Press", "Chest Exercise");

            //Act
            var response = await _httpClient.PostAsJsonAsync("/Exercises", exercise);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var id = await response.Content.ReadFromJsonAsync<Guid>();
            id.Should().NotBeEmpty();

            response.Headers.Location.Should().NotBeNull();
            response.Headers.Location.AbsolutePath.Should().Be($"/Exercises/{id}");
        }

        [Fact]
        public async Task Create_ReturnsValidationError_WhenExerciseIsInvalid()
        {
            //Arrange
            var exercise = new CreateExerciseRequest(string.Empty, new string('A', 501));

            //Act
            var response = await _httpClient.PostAsJsonAsync("/Exercises", exercise);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
            content.Should().NotBeNull();
            content.Errors.Should().NotBeNull();
            
            content.Errors.Should().ContainKey("Name")
                .WhoseValue.Should().Contain("Exercise name is required");
            content.Errors.Should().ContainKey("Description")
                .WhoseValue.Contains("Exercise description cannot exceed 500 characters");
        }

        [Fact]
        public async Task GetById_ReturnsExercise_WhenExerciseExists()
        {
            //Arrange
            var exercise = new CreateExerciseRequest("Bench Press", "Chest Exercise");

            var createResponse = await _httpClient.PostAsJsonAsync("/Exercises", exercise);
            var id = await createResponse.Content.ReadFromJsonAsync<Guid>();

            //Act
            var getResponse = await _httpClient.GetAsync($"/Exercises/{id}");

            //Assert
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var getResponseContent = await getResponse.Content.ReadFromJsonAsync<ExerciseResponse>();
            getResponseContent.Should().NotBeNull();
            getResponseContent.Id.Should().Be(id);
            getResponseContent.Name.Should().Be(exercise.Name);
            getResponseContent.Description.Should().Be(exercise.Description);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenExerciseDoesNotExist()
        {
            //Arrange
            var id = Guid.NewGuid();

            //Act
            var getResponse = await _httpClient.GetAsync($"/Exercises/{id}");

            //Assert
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetAll_ReturnsEmptyList_WhenNoExercisesExist()
        {
            //Act
            var getResponse = await _httpClient.GetAsync($"/Exercises");

            //Assert
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await getResponse.Content.ReadFromJsonAsync<IEnumerable<ExerciseResponse>>();
            content.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAll_ReturnsAllExercises_WhenExercisesExist()
        {
            //Arrange
            var exercise1 = new CreateExerciseRequest("Bench Press", "Chest Exercise");
            var exercise2 = new CreateExerciseRequest("Squat", "Quad Exercise");
            
            var createResponse1 = await _httpClient.PostAsJsonAsync("/Exercises", exercise1);
            var id1 = await createResponse1.Content.ReadFromJsonAsync<Guid>();
            var createResponse2 = await _httpClient.PostAsJsonAsync("/Exercises", exercise2);
            var id2 = await createResponse2.Content.ReadFromJsonAsync<Guid>();

            //Act
            var getResponse = await _httpClient.GetAsync($"/Exercises");

            //Assert
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await getResponse.Content.ReadFromJsonAsync<IEnumerable<ExerciseResponse>>();
            content.Should().HaveCount(2);
            content.Should().Contain(e => e.Id == id1);
            content.Should().Contain(e => e.Id == id2);
        }

        [Fact]
        public async Task Update_ReturnsUpdatedExercise_WhenExerciseExistsAndRequestIsValid()
        {
            //Arrange
            var exercise = new CreateExerciseRequest("Bench Press", "Chest Exercise");
            var updatedExercise = new CreateExerciseRequest("Squat", "Quad Exercise");

            var createResponse = await _httpClient.PostAsJsonAsync("/Exercises", exercise);
            var id = await createResponse.Content.ReadFromJsonAsync<Guid>();

            //Act
            var updateResponse = await _httpClient.PutAsJsonAsync($"/Exercises/{id}", updatedExercise);

            //Assert
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await updateResponse.Content.ReadFromJsonAsync<ExerciseResponse?>();
        }

        [Fact]
        public async Task Update_ReturnsValidationError_WhenExerciseExistsAndRequestIsInvalid()
        {
            //Arrange
            var exercise = new CreateExerciseRequest("Bench Press", "Chest Exercise");
            var updatedExercise = new CreateExerciseRequest(string.Empty, new string('A', 501));

            var createResponse = await _httpClient.PostAsJsonAsync("/Exercises", exercise);
            var id = await createResponse.Content.ReadFromJsonAsync<Guid>();

            //Act
            var updateResponse = await _httpClient.PutAsJsonAsync($"/Exercises/{id}", updatedExercise);

            //Assert
            updateResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var content = await updateResponse.Content.ReadFromJsonAsync<ValidationProblemDetails>();
            content.Should().NotBeNull();
            content.Errors.Should().NotBeNull();

            content.Errors.Should().ContainKey("Name")
                .WhoseValue.Should().Contain("Exercise name is required");
            content.Errors.Should().ContainKey("Description")
                .WhoseValue.Contains("Exercise description cannot exceed 500 characters");
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenExerciseDoesNotExists()
        {
            //Arrange
            var id = Guid.NewGuid();
            var updatedExercise = new CreateExerciseRequest("Squat", "Quad Exercise");

            //Act
            var updateResponse = await _httpClient.PutAsJsonAsync($"/Exercises/{id}", updatedExercise);

            //Assert
            updateResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenExerciseExists()
        {
            //Arrange
            var exercise = new CreateExerciseRequest("Bench Press", "Chest Exercise");

            var createResponse = await _httpClient.PostAsJsonAsync("/Exercises", exercise);
            var id = await createResponse.Content.ReadFromJsonAsync<Guid>();

            //Act
            var updateResponse = await _httpClient.DeleteAsync($"/Exercises/{id}");

            //Assert
            updateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenExerciseDoesNotExists()
        {
            //Arrange
            var id = Guid.NewGuid();

            //Act
            var updateResponse = await _httpClient.DeleteAsync($"/Exercises/{id}");

            //Assert
            updateResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        public Task InitializeAsync() => Task.CompletedTask;

        public async Task DisposeAsync()
        {
            await _factory.ResetDatabaseAsync();
        }
    }
}