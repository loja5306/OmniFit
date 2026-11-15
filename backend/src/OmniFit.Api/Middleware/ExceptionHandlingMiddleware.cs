using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OmniFit.Domain.Exceptions;
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

            if (exception is BusinessRuleException)
            {
                await HandleValidationException(httpContext, new ValidationException([new ValidationFailure(((BusinessRuleException)exception).PropertyName, exception.Message)]));
                return;
            }

            if (exception is AuthenticationException)
            {
                await HandleAuthenticationException(httpContext, (AuthenticationException)exception);
                return;
            }

            if (exception is DbUpdateConcurrencyException)
            {
                await HandleConcurrencyException(httpContext, (DbUpdateConcurrencyException)exception);
                return;
            }

            var method = SanitizeForLog(httpContext.Request.Method);
            var path = SanitizeForLog(httpContext.Request.Path.Value);

            _logger.LogError(exception, "Unhandled exception for {Method} {Path}", 
                method, path);
                
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
            var method = SanitizeForLog(httpContext.Request.Method);
            var path = SanitizeForLog(httpContext.Request.Path.Value);

            _logger.LogWarning("Validation error for {Method} {Path}", 
                method, path);

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

        private async Task HandleAuthenticationException(HttpContext httpContext, AuthenticationException exception)
        {
            var method = SanitizeForLog(httpContext.Request.Method);
            var path = SanitizeForLog(httpContext.Request.Path.Value);

            _logger.LogWarning("Authentication error for {Method} {Path}", 
                method, path);

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Unauthorized",
                Detail = exception.Message
            };

            httpContext.Response.StatusCode = problemDetails.Status.Value;
            await httpContext.Response.WriteAsJsonAsync(problemDetails);
        }

        private async Task HandleConcurrencyException(HttpContext httpContext, DbUpdateConcurrencyException exception)
        {
            var method = SanitizeForLog(httpContext.Request.Method);
            var path = SanitizeForLog(httpContext.Request.Path.Value);

            _logger.LogWarning("Concurrency error for {Method} {Path}", 
                method, path);

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Conflict",
                Detail = "This item has recently been changed. Please refresh and try again."
            };

            httpContext.Response.StatusCode = problemDetails.Status.Value;
            await httpContext.Response.WriteAsJsonAsync(problemDetails);
        }

        private static string SanitizeForLog(string? value)
        {
            return (value ?? string.Empty)
                .Replace("\r", string.Empty)
                .Replace("\n", string.Empty);
        }
    }
}
