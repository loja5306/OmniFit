using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using OmniFit.Api.Middleware;

namespace OmniFit.Api.Tests.Unit.Middleware
{
    public class ExceptionHandlingMiddlewareTests
    {
        private readonly HttpContext _httpContext = new DefaultHttpContext();

        [Fact]
        public async Task InvokeAsync_ShouldReturn500_WhenGenericExceptionThrown()
        {
            // Arrange
            RequestDelegate next = (ctx) => throw new Exception("An error occurred");
            var sut = new ExceptionHandlingMiddleware(next);

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
            var sut = new ExceptionHandlingMiddleware(next);

            // Act
            await sut.InvokeAsync(_httpContext);

            //Assert
            _httpContext.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }
    }
}
