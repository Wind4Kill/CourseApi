using CourseApi.Api.FiltrationClasses;
using FluentValidation;

namespace CourseApi.Api.EndpointFilters
{
    public class FiltrationEndpointFilter : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var filterOptions = context.Arguments.OfType<Filtering>().Single();
            await using (var scope = context.HttpContext.RequestServices.CreateAsyncScope())
            {
                var validator = scope.ServiceProvider.GetRequiredService<IValidator<Filtering>>();
                var validationResult = validator.Validate(filterOptions);
                if(!validationResult.IsValid)
                {
                    return TypedResults.ValidationProblem(validationResult.ToDictionary());
                }
            }
            return await next(context);
        }
    }
}