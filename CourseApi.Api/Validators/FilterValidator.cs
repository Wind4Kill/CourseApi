using CourseApi.Api.FiltrationClasses;
using CourseApi.Application.Filtration.HelpClasses;
using FluentValidation;

namespace CourseApi.Api.Validators
{
    public class FilterValidator:AbstractValidator<Filtering>
    {
        public FilterValidator()
        {
            RuleFor(f => f.PageNum).GreaterThan(1).When(f=>f.PageNum.HasValue);
            RuleFor(f => f.Filter).Must((filterOptions, ft) =>
            {
                if (ft is not null && ft != FilterOptions.Default.ToString()
                && filterOptions.FilterValue is null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }).WithMessage("Filter value must be provided if filtration type differs from default.");
        }
    }
}