using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseApiServices.Interfaces.HelpClasses;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CourseApi.Api
{
    public class CustomExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var (statusCode, message) = exception switch
            {
                EntityNotFoundException => (StatusCodes.Status400BadRequest, "Entity Not Found"),
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

            return true;
        }
    }
}