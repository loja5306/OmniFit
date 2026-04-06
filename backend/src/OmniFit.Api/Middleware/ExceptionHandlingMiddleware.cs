using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;

namespace OmniFit.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        public async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            if (exception is ValidationException)
            {
                await HandleValidationException(httpContext, (ValidationException)exception);
                return;
            }

            if (exception is AuthenticationException)
            {
                await HandleAuthenicationException(httpContext, (AuthenticationException)exception);
                return;
            }

            _logger.LogError(exception, "Unhandled exception for {Method} {Path}", 
                httpContext.Request.Method, httpContext.Request.Path);
                
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Server Error",
                Detail = "An unexpected error occured. Please try again."
            };

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await httpContext.Response.WriteAsJsonAsync(problemDetails);
        }

        private async Task HandleValidationException(HttpContext httpContext, ValidationException exception)
        {
            _logger.LogWarning("Validation error for {Method} {Path}", 
                httpContext.Request.Method, httpContext.Request.Path);

            var problemDetails = new ValidationProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation Error",
                Errors = exception.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key, 
                        g => g.ToList().Select(e => e.ErrorMessage).ToArray()
                    )
            };

            httpContext.Response.StatusCode = problemDetails.Status.Value;
            await httpContext.Response.WriteAsJsonAsync(problemDetails);
        }

        private async Task HandleAuthenicationException(HttpContext httpContext, AuthenticationException exception)
        {
            _logger.LogWarning("Authentication error for {Method} {Path}", 
                httpContext.Request.Method, httpContext.Request.Path);

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Unauthorized",
                Detail = exception.Message
            };

            httpContext.Response.StatusCode = problemDetails.Status.Value;
            await httpContext.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}
