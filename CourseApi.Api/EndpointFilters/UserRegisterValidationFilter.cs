using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseApi.Application.Authentication;
using FluentValidation;

namespace CourseApi.Api.EndpointFilters
{
    public class UserRegisterValidationFilter : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            UserRegisterDto userCredentials = context.Arguments.OfType<UserRegisterDto>().Single();
            IValidator<UserRegisterDto> validator = context.HttpContext.RequestServices.GetRequiredService<IValidator<UserRegisterDto>>();
            var result = await validator.ValidateAsync(userCredentials);
            if (!result.IsValid)
            {
                return TypedResults.ValidationProblem(result.ToDictionary());
            }
            return await next(context);
        }
    }
}