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
        string? filter = context.GetArgument<string>(0);
        string? filtrationValue = context.GetArgument<string>(2);

        if ((filter is not null || filter != FilterOptions.Default.ToString()) && filtrationValue is not null)
        {
            return Results.ValidationProblem(new Dictionary<string, string[]>()
             {
                {"filtrationValue", ["Filtration type was chosen not as default, but filtration value wasn't provided"] }
             });
        }
        return await next(context);
    }
}
