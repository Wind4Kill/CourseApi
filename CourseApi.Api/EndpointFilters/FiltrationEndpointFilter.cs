using CourseApi.Api.FiltrationClasses;
using FluentValidation;

namespace CourseApi.Api.EndpointFilters
{
    public class FiltrationEndpointFilter : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var filterOptions = context.Arguments.OfType<Filtering>().Single();

            var validator = context.HttpContext.RequestServices.GetRequiredService<IValidator<Filtering>>();
                
                var validationResult = validator.Validate(filterOptions);
                if(!validationResult.IsValid)
                {
                    return TypedResults.ValidationProblem(validationResult.ToDictionary());
                }
            
            return await next(context);
        }
    }
}