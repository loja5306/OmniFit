using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using OmniFit.Api.Middleware;
using System.Security.Authentication;

namespace OmniFit.Api.Tests.Unit.Middleware
{
    public class ExceptionHandlingMiddlewareTests
    {
        private readonly HttpContext _httpContext = new DefaultHttpContext();
        private readonly ILogger<ExceptionHandlingMiddleware> _logger = Substitute.For<ILogger<ExceptionHandlingMiddleware>>();

        [Fact]
        public async Task InvokeAsync_ShouldReturn500_WhenGenericExceptionThrown()
        {
            // Arrange
            RequestDelegate next = (ctx) => throw new Exception("An error occurred");
            var sut = new ExceptionHandlingMiddleware(next, _logger);

            // Act
            await sut.InvokeAsync(_httpContext);

            //Assert
            _httpContext.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [Fact]
        public async Task InvokeAsync_ShouldReturn400_WhenValidationExceptionThrown()
        {
            // Arrange
            RequestDelegate next = (ctx) => throw new ValidationException(new List<ValidationFailure> { new ValidationFailure("Name", "Workout name is required") });
            var sut = new ExceptionHandlingMiddleware(next, _logger);

            // Act
            await sut.InvokeAsync(_httpContext);

            //Assert
            _httpContext.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task InvokeAsync_ShouldReturn401_WhenAuthenticationExceptionThrown()
        {
            // Arrange
            RequestDelegate next = (ctx) => throw new AuthenticationException("The email and/or password was incorrect");
            var sut = new ExceptionHandlingMiddleware(next, _logger);

            // Act
            await sut.InvokeAsync(_httpContext);

            //Assert
            _httpContext.Response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }
    }
}
