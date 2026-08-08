using System;
namespace CourseApi.Domain.HelpClasses;

public class SortFilterOptions
{
      public FilterOptions Filter { get; set; }
      public SortingOptions Sorting { get; set; }

      public string? FilterValue { get; set; }

      public int PageNum { get; set; }

      public SortFilterOptions(string? sortingOptions,
       string? filterOptions, string? filterValue, int? page)
      {
            Sorting = Enum.TryParse<SortingOptions>(sortingOptions, true, out SortingOptions sorting) ? sorting : SortingOptions.Default;

            Filter = Enum.TryParse<FilterOptions>(filterOptions, true, out FilterOptions filtering) ? filtering : FilterOptions.Default;

            FilterValue = filterValue;

            PageNum = page.HasValue ? page.Value : 1;
      }

}
