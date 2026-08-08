using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseApi.Domain.HelpClasses;

namespace CourseApi.Api.FiltrationClasses;

public class FiltrationFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        Filtering filter = context.GetArgument<Filtering>(1);
        string? filterType = filter.Filter;
        string? filtrationValue = filter.FilterValue;

        if ((filter is not null || filterType != FilterOptions.Default.ToString()) && filtrationValue is null)
        {
            return Results.ValidationProblem(new Dictionary<string, string[]>()
             {
                {"filtrationValue", ["Filtration type was chosen not as default, but filtration value wasn't provided"] }
             });
        }
        return await next(context);
    }
}
