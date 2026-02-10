using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using OmniFit.Application.DTOs;
using OmniFit.Domain.Entities;
using OmniFit.Infrastructure.Data;
using System.Net;
using System.Net.Http.Json;

namespace OmniFit.Api.Tests.Integration.Controllers
{
    public class WorkoutsControllerTests : IClassFixture<ApiFactory>, IAsyncLifetime
    {
        private readonly ApiFactory _factory;
        private readonly HttpClient _httpClient;

        public WorkoutsControllerTests(ApiFactory factory)
        {
            _factory = factory;
            _httpClient = factory.HttpClient;
        }

        [Fact]
        public async Task Create_CreatesWorkout_WhenWorkoutIsValid()
        {
            //Arrange
            var workout = new CreateWorkoutRequest("Monday Workout",
                new List<WorkoutExerciseRequest>
                {
                    new WorkoutExerciseRequest(TestData.Exercises.BenchPressExercise.Id, new List<WorkoutSetRequest>
                    {
                        new WorkoutSetRequest(1, 5, 80),
                        new WorkoutSetRequest(2, 6, 70),
                        new WorkoutSetRequest(3, 5, 70),
                    }),
                    new WorkoutExerciseRequest(TestData.Exercises.SquatExercise.Id, new List<WorkoutSetRequest>
                    {
                        new WorkoutSetRequest(1, 5, 80),
                        new WorkoutSetRequest(2, 5, 75),
                        new WorkoutSetRequest(3, 3, 75),
                    })
                });

            //Act
            var response = await _httpClient.PostAsJsonAsync("/Workouts", workout);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var id = await response.Content.ReadFromJsonAsync<Guid>();
            id.Should().NotBeEmpty();

            response.Headers.Location.Should().NotBeNull();
            response.Headers.Location.AbsolutePath.Should().Be($"/Workouts/{id}");
        }

        [Fact]
        public async Task Create_ReturnsValidationError_WhenWorkoutIsInvalid()
        {
            //Arrange
            var workout = new CreateWorkoutRequest(string.Empty,
                new List<WorkoutExerciseRequest>
                {
                    new WorkoutExerciseRequest(TestData.Exercises.SquatExercise.Id, new List<WorkoutSetRequest>
                    {
                        new WorkoutSetRequest(1, 5, -80),
                        new WorkoutSetRequest(2, -6, 70),
                        new WorkoutSetRequest(3, 5, 70),
                    }),
                    new WorkoutExerciseRequest(TestData.Exercises.BenchPressExercise.Id, new List<WorkoutSetRequest>())
                });

            //Act
            var response = await _httpClient.PostAsJsonAsync("/Workouts", workout);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
            content.Should().NotBeNull();
            content.Errors.Should().NotBeNull();

            content.Errors.Should().ContainKey("Name")
                .WhoseValue.Should().Contain("Workout name is required");
            content.Errors.Should().Contain(e => 
                e.Key.EndsWith("Sets") && 
                e.Value.Contains("Each exercise must have at least 1 set"));
            content.Errors.Should().Contain(e =>
                e.Key.EndsWith("Weight") && 
                e.Value.Contains("Weight cannot be negative"));
            content.Errors.Should().Contain(e =>
                e.Key.EndsWith("Reps") && 
                e.Value.Contains("Reps cannot be negative"));
        }

        [Fact]
        public async Task GetById_ReturnsWorkout_WhenWorkoutExists()
        {
            //Arrange
            var workout = new CreateWorkoutRequest("Monday Workout",
                new List<WorkoutExerciseRequest>
                {
                    new WorkoutExerciseRequest(TestData.Exercises.SquatExercise.Id, new List<WorkoutSetRequest>
                    {
                        new WorkoutSetRequest(1, 5, 80),
                        new WorkoutSetRequest(2, 6, 70),
                        new WorkoutSetRequest(3, 5, 70),
                    }),
                    new WorkoutExerciseRequest(TestData.Exercises.BenchPressExercise.Id, new List<WorkoutSetRequest>
                    {
                        new WorkoutSetRequest(1, 5, 80),
                        new WorkoutSetRequest(2, 5, 75),
                        new WorkoutSetRequest(3, 3, 75),
                    })
                });

            var response = await _httpClient.PostAsJsonAsync("/Workouts", workout);
            var id = await response.Content.ReadFromJsonAsync<Guid>();

            //Act
            var getResponse = await _httpClient.GetAsync($"/Workouts/{id}");

            //Assert
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var getResponseContent = await getResponse.Content.ReadFromJsonAsync<WorkoutResponse>();
            getResponseContent.Should().NotBeNull();
            getResponseContent.Id.Should().Be(id);
            getResponseContent.Name.Should().Be(workout.Name);
            getResponseContent.TotalExercises.Should().Be(2);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenWorkoutDoesNotExist()
        {
            //Arrange
            var id = Guid.NewGuid();

            //Act
            var getResponse = await _httpClient.GetAsync($"/Workouts/{id}");

            //Assert
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetAll_ReturnsEmptyList_WhenNoWorkoutsExist()
        {
            //Act
            var getResponse = await _httpClient.GetAsync($"/Workouts");

            //Assert
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await getResponse.Content.ReadFromJsonAsync<IEnumerable<WorkoutResponse>>();
            content.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAll_ReturnsAllWorkouts_WhenWorkoutsExist()
        {
            //Arrange
            var workout1 = new CreateWorkoutRequest("Monday Workout",
                new List<WorkoutExerciseRequest>
                {
                    new WorkoutExerciseRequest(TestData.Exercises.SquatExercise.Id, new List<WorkoutSetRequest>
                    {
                        new WorkoutSetRequest(1, 5, 80),
                        new WorkoutSetRequest(2, 6, 70),
                        new WorkoutSetRequest(3, 5, 70),
                    }),
                    new WorkoutExerciseRequest(TestData.Exercises.BenchPressExercise.Id, new List<WorkoutSetRequest>
                    {
                        new WorkoutSetRequest(1, 5, 80),
                        new WorkoutSetRequest(2, 5, 75),
                        new WorkoutSetRequest(3, 3, 75),
                    })
                });
            var workout2 = new CreateWorkoutRequest("Wednesday Workout",
                new List<WorkoutExerciseRequest>
                {
                    new WorkoutExerciseRequest(TestData.Exercises.SquatExercise.Id, new List<WorkoutSetRequest>
                    {
                        new WorkoutSetRequest(1, 6, 80),
                        new WorkoutSetRequest(2, 7, 70),
                        new WorkoutSetRequest(3, 5, 70),
                    }),
                    new WorkoutExerciseRequest(TestData.Exercises.BenchPressExercise.Id, new List<WorkoutSetRequest>
                    {
                        new WorkoutSetRequest(1, 6, 80),
                        new WorkoutSetRequest(2, 5, 75),
                        new WorkoutSetRequest(3, 5, 75),
                    })
                });

            var response1 = await _httpClient.PostAsJsonAsync("/Workouts", workout1);
            var id1 = await response1.Content.ReadFromJsonAsync<Guid>();
            var response2 = await _httpClient.PostAsJsonAsync("/Workouts", workout2);
            var id2 = await response2.Content.ReadFromJsonAsync<Guid>();

            //Act
            var getResponse = await _httpClient.GetAsync($"/Workouts");

            //Assert
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await getResponse.Content.ReadFromJsonAsync<IEnumerable<WorkoutResponse>>();
            content.Should().HaveCount(2);
            content.Should().Contain(e => e.Id == id1);
            content.Should().Contain(e => e.Id == id2);
        }
        public async Task InitializeAsync()
        {
            await SeedExerciseAsync(TestData.Exercises.BenchPressExercise);
            await SeedExerciseAsync(TestData.Exercises.SquatExercise);

            await SeedUserAsync(TestData.Users.User);
        }

        public async Task DisposeAsync()
        {
            await _factory.ResetDatabaseAsync();
        }

        #region Seeding Data

        private async Task SeedExerciseAsync(Exercise exercise)
        {
            using var scope = _factory.Services.CreateScope();

            var _dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            _dbContext.Exercises.Add(exercise);
            await _dbContext.SaveChangesAsync();
        }

        private async Task SeedUserAsync(IdentityUser user)
        {
            using var scope = _factory.Services.CreateScope();

            var _dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
        }

        #endregion
    }
}
