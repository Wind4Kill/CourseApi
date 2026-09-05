using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseApi.Application.DTOs.CourseDtos;
using FluentValidation;

namespace CourseApi.Api.EndpointFilters
{
    public class CreateCourseFilter : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            CreateCourseDto createdCourse = context.Arguments.OfType<CreateCourseDto>().Single();

            IValidator<CreateCourseDto> validator = context.HttpContext.
            RequestServices.GetRequiredService<IValidator<CreateCourseDto>>();

            var validationResult = validator.Validate(createdCourse);
            if (!validationResult.IsValid)
            {
                return TypedResults.ValidationProblem(validationResult.ToDictionary());
            }

            return await next(context);
        }
    }
}