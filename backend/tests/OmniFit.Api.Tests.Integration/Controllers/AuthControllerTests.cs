
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Newtonsoft.Json.Linq;
using OmniFit.Application.DTOs;
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;

namespace OmniFit.Api.Tests.Integration.Controllers
{
    public class AuthControllerTests : IClassFixture<ApiFactory>, IAsyncLifetime
    {
        private readonly ApiFactory _factory;
        private readonly HttpClient _httpClient;

        public AuthControllerTests(ApiFactory factory)
        {
            _factory = factory;
            _httpClient = factory.HttpClient;
        }

        [Fact]
        public async Task Register_ShouldReturnToken_WhenUserIsValid()
        {
            //Arrange
            var request = new RegisterRequestDto("lukeatkinson@gmail.com", "Password123!");

            //Act
            var response = await _httpClient.PostAsJsonAsync("Auth/Register", request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadFromJsonAsync<AuthResponseDto>();

            content.Should().NotBeNull();

            var handler = new JsonWebTokenHandler();
            var jwtToken = handler.ReadJsonWebToken(content.Token);

            jwtToken.GetClaim(JwtRegisteredClaimNames.Email).Value.Should().Be(request.Email);
        }

        [Fact]
        public async Task Register_ShouldReturnValidationError_WhenUserIsInvalid()
        {
            //Arrange
            var request = new RegisterRequestDto("lukeatkinson@gmail.com", "Pass1");

            //Act
            var response = await _httpClient.PostAsJsonAsync("Auth/Register", request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();

            content.Should().NotBeNull();
            content.Errors.Should().Contain(e => e.Key == "Registration" && 
                e.Value.Contains("Passwords must be at least 6 characters."));
        }

        [Fact]
        public async Task Login_ShouldReturnToken_WhenUserIsValid()
        {
            //Arrange
            var request = new RegisterRequestDto("lukeatkinson@gmail.com", "Password123!");
            await _httpClient.PostAsJsonAsync("Auth/Register", request);

            //Act
            var response = await _httpClient.PostAsJsonAsync("Auth/Login", request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadFromJsonAsync<AuthResponseDto>();

            content.Should().NotBeNull();

            var handler = new JsonWebTokenHandler();
            var jwtToken = handler.ReadJsonWebToken(content.Token);

            jwtToken.GetClaim(JwtRegisteredClaimNames.Email).Value.Should().Be(request.Email);
        }

        [Fact]
        public async Task Login_ShouldReturnUnauthorized_WhenUserDoesNotExist()
        {
            //Arrange
            var request = new RegisterRequestDto("lukeatkinson@gmail.com", "Password123!");

            //Act
            var response = await _httpClient.PostAsJsonAsync("Auth/Login", request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            var content = await response.Content.ReadFromJsonAsync<ProblemDetails>();

            content.Should().NotBeNull();
            content.Detail.Should().Be("The email and/or password was incorrect");
        }

        public Task InitializeAsync() => Task.CompletedTask;
        
        public async Task DisposeAsync()
        {
            await _factory.ResetDatabaseAsync();
        }
    }
}
