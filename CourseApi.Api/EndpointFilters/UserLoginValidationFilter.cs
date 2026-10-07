using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseApi.Application.Authentication.DTOs;
using FluentValidation;

namespace CourseApi.Api.EndpointFilters
{
    public class UserLoginValidationFilter : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            UserLoginDto userCredentials = context.Arguments.OfType<UserLoginDto>().Single();
            var validator = context.HttpContext.RequestServices.GetRequiredService<IValidator<UserLoginDto>>();

            var result = await validator.ValidateAsync(userCredentials);

            if (!result.IsValid)
            {
                return TypedResults.ValidationProblem(result.ToDictionary());
            }

            return await next(context);
        }
    }
}