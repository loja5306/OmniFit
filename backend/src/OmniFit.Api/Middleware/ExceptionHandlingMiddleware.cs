using FluentValidation;

namespace OmniFit.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
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
                
            var problemDetails = new
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Server Error",
                Detail = "An unexpected error occured. Please try again."
            };

            httpContext.Response.StatusCode = problemDetails.Status;

            await httpContext.Response.WriteAsJsonAsync(problemDetails);
        }

        private async Task HandleValidationException(HttpContext httpContext, ValidationException exception)
        {
            var problemDetails = new
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation Error",
                Errors = exception.Errors.Select(x => new
                {
                    PropertyName = x.PropertyName,
                    Message = x.ErrorMessage
                })
            };

            httpContext.Response.StatusCode = problemDetails.Status;
            await httpContext.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}
