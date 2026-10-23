using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OmniFit.Application.DTOs.Workouts;
using OmniFit.Domain.Common;
using OmniFit.Domain.Entities;
using OmniFit.Domain.Enums;
using OmniFit.Infrastructure.Data;
using System.Net;
using System.Net.Http.Json;

namespace OmniFit.Api.Tests.Integration.Controllers
{
    public class WorkoutsControllerTests : IClassFixture<ApiFactory>, IAsyncLifetime
    {
        private readonly ApiFactory _factory;
        private readonly HttpClient _adminClient;
        private readonly HttpClient _userClient;

        public WorkoutsControllerTests(ApiFactory factory)
        {
            _factory = factory;
            _adminClient = factory.CreateClientForUser(TestData.Users.AdminUser.Id, ["Admin"]);
            _userClient = factory.CreateClientForUser(TestData.Users.StandardUser.Id, []);
        }

        [Fact]
        public async Task Create_CreatesWorkout_WhenWorkoutIsValid()
        {
            //Arrange
            var workout = new WorkoutRequest("Monday Workout");

            //Act
            var response = await _adminClient.PostAsJsonAsync("/Workouts", workout);

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
            var workout = new WorkoutRequest(string.Empty);

            //Act
            var response = await _adminClient.PostAsJsonAsync("/Workouts", workout);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
            content.Should().NotBeNull();
            content.Errors.Should().NotBeNull();

            content.Errors.Should().ContainKey("Name")
                .WhoseValue.Should().Contain("Workout name is required");
        }

        [Fact]
        public async Task GetById_ReturnsWorkout_WhenWorkoutExists()
        {
            //Arrange
            var workout = new WorkoutRequest("Monday Workout");

            var response = await _adminClient.PostAsJsonAsync("/Workouts", workout);
            var id = await response.Content.ReadFromJsonAsync<Guid>();

            //Act
            var getResponse = await _adminClient.GetAsync($"/Workouts/{id}");

            //Assert
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await getResponse.Content.ReadFromJsonAsync<WorkoutResponse>();
            content.Should().NotBeNull();
            content.Id.Should().Be(id);
            content.Name.Should().Be(workout.Name);
            content.Status.Should().Be(WorkoutStatus.InProgress);
            content.CompletedOn.Should().BeNull();
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenWorkoutDoesNotExist()
        {
            //Arrange
            var id = Guid.NewGuid();

            //Act
            var getResponse = await _adminClient.GetAsync($"/Workouts/{id}");

            //Assert
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenWorkoutDoesNotBelongToUser()
        {
            //Arrange
            var workout = new WorkoutRequest("Monday Workout");

            var response = await _adminClient.PostAsJsonAsync("/Workouts", workout);
            var id = await response.Content.ReadFromJsonAsync<Guid>();

            //Act
            var getResponse = await _userClient.GetAsync($"/Workouts/{id}");

            //Assert
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetById_ReturnsWorkout_WhenAdminRequestsWorkoutBelongingToAnotherUser()
        {
            //Arrange
            var workout = new WorkoutRequest("Monday Workout");

            var response = await _userClient.PostAsJsonAsync("/Workouts", workout);
            var id = await response.Content.ReadFromJsonAsync<Guid>();

            //Act
            var getResponse = await _adminClient.GetAsync($"/Workouts/{id}");

            //Assert
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await getResponse.Content.ReadFromJsonAsync<WorkoutResponse>();
            content.Should().NotBeNull();
            content.Id.Should().Be(id);
            content.Name.Should().Be(workout.Name);
        }

        [Fact]
        public async Task GetForUser_ReturnsOnlyUsersWorkouts_WhenUserIsNotAnAdmin()
        {
            //Arrange
            var workoutRequest1 = new WorkoutRequest("Monday Workout");
            var workoutRequest2 = new WorkoutRequest("Tuesday Workout");
            var workoutRequest3 = new WorkoutRequest("Wednesday Workout");
            var workoutRequest4 = new WorkoutRequest("Thursday Workout");

            var workoutResponse1 = await _userClient.PostAsJsonAsync($"/Workouts", workoutRequest1);
            var workoutId1 = await workoutResponse1.Content.ReadFromJsonAsync<Guid>();
            var workoutResponse2 = await _userClient.PostAsJsonAsync($"/Workouts", workoutRequest2);
            var workoutId2 = await workoutResponse2.Content.ReadFromJsonAsync<Guid>();
            var workoutResponse3 = await _adminClient.PostAsJsonAsync($"/Workouts", workoutRequest3);
            var workoutId3 = await workoutResponse3.Content.ReadFromJsonAsync<Guid>();
            var workoutResponse4 = await _adminClient.PostAsJsonAsync($"/Workouts", workoutRequest4);
            var workoutId4 = await workoutResponse4.Content.ReadFromJsonAsync<Guid>();

            //Act
            var getForUserResponse = await _userClient.GetAsync("/Workouts/Me");

            //Assert
            getForUserResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await getForUserResponse.Content.ReadFromJsonAsync<PagedResult<WorkoutResponse>>();
            content.Should().NotBeNull();
            content.TotalCount.Should().Be(2);
            var workoutIds = content.Items.Select(w => w.Id);
            workoutIds.Should().Contain(workoutId1);
            workoutIds.Should().Contain(workoutId2);
            workoutIds.Should().NotContain(workoutId3);
            workoutIds.Should().NotContain(workoutId4);
        }

        [Fact]
        public async Task GetAll_ReturnsEmptyList_WhenNoWorkoutsExist()
        {
            //Act
            var getResponse = await _adminClient.GetAsync("/Workouts");

            //Assert
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await getResponse.Content.ReadFromJsonAsync<PagedResult<WorkoutResponse>>();
            content.Should().NotBeNull();
            content.Items.Should().BeEmpty();
            content.TotalCount.Should().Be(0);
            content.Page.Should().Be(1);
            content.PageSize.Should().Be(20);
        }

        [Fact]
        public async Task GetAll_ReturnsAllWorkouts_WhenWorkoutsExist()
        {
            //Arrange
            var workout1 = new WorkoutRequest("Monday Workout");
            var workout2 = new WorkoutRequest("Wednesday Workout");

            var response1 = await _adminClient.PostAsJsonAsync("/Workouts", workout1);
            var id1 = await response1.Content.ReadFromJsonAsync<Guid>();
            var response2 = await _adminClient.PostAsJsonAsync("/Workouts", workout2);
            var id2 = await response2.Content.ReadFromJsonAsync<Guid>();

            //Act
            var getResponse = await _adminClient.GetAsync("/Workouts");

            //Assert
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await getResponse.Content.ReadFromJsonAsync<PagedResult<WorkoutResponse>>();
            content.Should().NotBeNull();
            content.Items.Should().HaveCount(2);
            content.Items.Should().Contain(e => e.Id == id1);
            content.Items.Should().Contain(e => e.Id == id2);
            content.TotalCount.Should().Be(2);
            content.Page.Should().Be(1);
            content.PageSize.Should().Be(20);
        }

        [Fact]
        public async Task GetAll_ReturnsForbidden_WhenUserIsNotAdmin()
        {
            //Act
            var response = await _userClient.GetAsync("/Workouts");
            
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Complete_ReturnsBadRequest_WhenWorkoutHasNoSets()
        {
            //Arrange
            var workoutRequest = new WorkoutRequest("Monday Workout");
            var workoutExerciseRequest = new WorkoutExerciseRequest(TestData.Exercises.BenchPressExercise.Id);

            var workoutResponse = await _adminClient.PostAsJsonAsync("/Workouts", workoutRequest);
            var workoutId = await workoutResponse.Content.ReadFromJsonAsync<Guid>();
            await _adminClient.PostAsJsonAsync($"/Workouts/{workoutId}/Exercises", workoutExerciseRequest);
            
            //Act
            var completeWorkoutResponse = await _adminClient.PostAsync($"/Workouts/{workoutId}/Complete", null);
            
            //Assert
            completeWorkoutResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var content = await completeWorkoutResponse.Content.ReadFromJsonAsync<ValidationProblemDetails>();
            content.Should().NotBeNull();
            content.Errors.Should().NotBeNull();

            content.Errors.Should().ContainKey("WorkoutExercises")
                .WhoseValue.Should().Contain("Each exercise must have at least 1 set");
        }

        [Fact]
        public async Task Complete_CompletesWorkout_WhenWorkoutHasExercisesAndSets()
        {
            //Arrange
            var workoutRequest = new WorkoutRequest("Monday Workout");
            var workoutExerciseRequest = new WorkoutExerciseRequest(TestData.Exercises.BenchPressExercise.Id);
            var workoutSetRequest = new WorkoutSetRequest(1, 10, 100);

            var workoutResponse = await _adminClient.PostAsJsonAsync("/Workouts", workoutRequest);
            var workoutId = await workoutResponse.Content.ReadFromJsonAsync<Guid>();
            var workoutExerciseResponse = await _adminClient.PostAsJsonAsync($"/Workouts/{workoutId}/Exercises", workoutExerciseRequest);
            var workoutExerciseId = await workoutExerciseResponse.Content.ReadFromJsonAsync<Guid>();
            await _adminClient.PostAsJsonAsync($"/Workouts/{workoutId}/Exercises/{workoutExerciseId}/Sets", workoutSetRequest);
            
            //Act
            var completeWorkoutResponse = await _adminClient.PostAsync($"/Workouts/{workoutId}/Complete", null);
            
            //Assert
            completeWorkoutResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var getWorkoutResponse = await _adminClient.GetAsync($"/Workouts/{workoutId}");
            var content = await getWorkoutResponse.Content.ReadFromJsonAsync<WorkoutResponse>();
            content.Should().NotBeNull();
            content.Status.Should().Be(WorkoutStatus.Completed);
        }
    
        [Fact]
        public async Task AddExercise_ReturnsBadRequest_WhenWorkoutCompleted()
        {
            //Arrange
            var workoutRequest = new WorkoutRequest("Monday Workout");
            var workoutExerciseRequest = new WorkoutExerciseRequest(TestData.Exercises.BenchPressExercise.Id);
            var workoutSetRequest = new WorkoutSetRequest(1, 10, 100);
            var workoutExerciseRequest2 = new WorkoutExerciseRequest(TestData.Exercises.SquatExercise.Id);

            var workoutResponse = await _adminClient.PostAsJsonAsync("/Workouts", workoutRequest);
            var workoutId = await workoutResponse.Content.ReadFromJsonAsync<Guid>();
            var workoutExerciseResponse = await _adminClient.PostAsJsonAsync($"/Workouts/{workoutId}/Exercises", workoutExerciseRequest);
            var workoutExerciseId = await workoutExerciseResponse.Content.ReadFromJsonAsync<Guid>();
            await _adminClient.PostAsJsonAsync($"/Workouts/{workoutId}/Exercises/{workoutExerciseId}/Sets", workoutSetRequest);
            await _adminClient.PostAsync($"/Workouts/{workoutId}/Complete", null);
            
            //Act
            var workoutExerciseResponse2 = await _adminClient.PostAsJsonAsync($"/Workouts/{workoutId}/Exercises", workoutExerciseRequest2);
            
            //Assert
            workoutExerciseResponse2.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var content = await workoutExerciseResponse2.Content.ReadFromJsonAsync<ValidationProblemDetails>();
            content.Should().NotBeNull();
            content.Errors.Should().NotBeNull();

            content.Errors.Should().ContainKey("Status")
                .WhoseValue.Should().Contain("Cannot modify a completed workout");
        }

        [Fact]
        public async Task RemoveExercise_RemovesExercisesAndSets_WhenRequestIsValid()
        {
            //Arrange
            var workoutRequest = new WorkoutRequest("Monday Workout");
            var workoutExerciseRequest = new WorkoutExerciseRequest(TestData.Exercises.BenchPressExercise.Id);
            var workoutSetRequest = new WorkoutSetRequest(1, 10, 100);

            var workoutResponse = await _adminClient.PostAsJsonAsync("/Workouts", workoutRequest);
            var workoutId = await workoutResponse.Content.ReadFromJsonAsync<Guid>();
            var workoutExerciseResponse = await _adminClient.PostAsJsonAsync($"/Workouts/{workoutId}/Exercises", workoutExerciseRequest);
            var workoutExerciseId = await workoutExerciseResponse.Content.ReadFromJsonAsync<Guid>();
            var workoutSetResponse = await _adminClient.PostAsJsonAsync($"/Workouts/{workoutId}/Exercises/{workoutExerciseId}/Sets", workoutSetRequest);
            var workoutSetId = await workoutSetResponse.Content.ReadFromJsonAsync<Guid>();

            //Act
            var removeExerciseResponse = await _adminClient.DeleteAsync($"/Workouts/{workoutId}/Exercises/{workoutExerciseId}");
            
            //Assert
            removeExerciseResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                (await dbContext.WorkoutExercises.AnyAsync(we => we.Id == workoutExerciseId)).Should().BeFalse();
                (await dbContext.WorkoutSets.AnyAsync(ws => ws.Id == workoutSetId)).Should().BeFalse();
            }
        }

        [Fact]
        public async Task AddSet_ReturnsNotFound_WhenWorkoutBelongsToAnotherUser()
        {
            //Arrange
            var workoutRequest = new WorkoutRequest("Monday Workout");
            var workoutExerciseRequest = new WorkoutExerciseRequest(TestData.Exercises.BenchPressExercise.Id);
            var workoutSetRequest = new WorkoutSetRequest(1, 10, 100);

            var workoutResponse = await _adminClient.PostAsJsonAsync("/Workouts", workoutRequest);
            var workoutId = await workoutResponse.Content.ReadFromJsonAsync<Guid>();
            var workoutExerciseResponse = await _adminClient.PostAsJsonAsync($"/Workouts/{workoutId}/Exercises", workoutExerciseRequest);
            var workoutExerciseId = await workoutExerciseResponse.Content.ReadFromJsonAsync<Guid>();

            //Act
            var workoutSetResponse = await _userClient.PostAsJsonAsync($"/Workouts/{workoutId}/Exercises/{workoutExerciseId}/Sets", workoutSetRequest);
            
            //Assert
            workoutSetResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateSet_UpdatesSet_WhenRequestIsValid()
        {
            //Arrange
            var workoutRequest = new WorkoutRequest("Monday Workout");
            var workoutExerciseRequest = new WorkoutExerciseRequest(TestData.Exercises.BenchPressExercise.Id);
            var workoutSetRequest = new WorkoutSetRequest(1, 10, 100);
            var updateSetRequest = new WorkoutSetRequest(1, 11, 110);

            var workoutResponse = await _adminClient.PostAsJsonAsync("/Workouts", workoutRequest);
            var workoutId = await workoutResponse.Content.ReadFromJsonAsync<Guid>();
            var workoutExerciseResponse = await _adminClient.PostAsJsonAsync($"/Workouts/{workoutId}/Exercises", workoutExerciseRequest);
            var workoutExerciseId = await workoutExerciseResponse.Content.ReadFromJsonAsync<Guid>();
            var workoutSetResponse = await _adminClient.PostAsJsonAsync($"/Workouts/{workoutId}/Exercises/{workoutExerciseId}/Sets", workoutSetRequest);
            var workoutSetId = await workoutSetResponse.Content.ReadFromJsonAsync<Guid>();
            
            //Act
            var updateSetResponse = await _adminClient.PutAsJsonAsync($"/Workouts/{workoutId}/Exercises/{workoutExerciseId}/Sets/{workoutSetId}", updateSetRequest);
            
            //Assert
            var content = await updateSetResponse.Content.ReadFromJsonAsync<WorkoutSetResponse>();
            content.Should().NotBeNull();
            content.SetNumber.Should().Be(updateSetRequest.SetNumber);
            content.Reps.Should().Be(updateSetRequest.Reps);
            content.Weight.Should().Be(updateSetRequest.Weight);
        }

        public async Task InitializeAsync()
        {
            await SeedExerciseAsync(TestData.Exercises.BenchPressExercise);
            await SeedExerciseAsync(TestData.Exercises.SquatExercise);

            await SeedUserAsync(TestData.Users.AdminUser);
            await SeedUserAsync(TestData.Users.StandardUser);
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
