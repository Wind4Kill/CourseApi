using CourseApi.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CourseApi.Api.HelpClasses
{
    public class CustomExceptionHandler : IExceptionHandler
    {
        readonly ILogger _logger;
        public CustomExceptionHandler(ILoggerFactory loggerFactory)
        {
           _logger = loggerFactory.CreateLogger("Exception handler");
        }
        
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var (statusCode, message) = exception switch
            {
                EntityNotFoundException => (StatusCodes.Status404NotFound, "Entity Not Found"),
                EntityAlreadyExistsExceptions=>(StatusCodes.Status400BadRequest, "Entity Already Exists"),
                _ => (StatusCodes.Status500InternalServerError, "Internal Server Error")
            };

            ProblemDetails details = new ProblemDetails()
            {
                Status = statusCode,
                Title = message,
                Detail = exception.Message
            };

            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(details, cancellationToken);
            _logger.LogError(exception, message);

            return true;
        }
    }
}