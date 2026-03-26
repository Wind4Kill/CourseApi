using System;
using CourseApiServices.HelpClasses;

namespace CourseApiServices.Interfaces.HelpClasses;

public class SortFilterOptions
{
      public FilterOptions Filter { get; set; } = default;
      public SortingOptions Sorting { get; set; } = default;

      public string? FilterValue { get; set; } = null;

      public int PageNum { get; set; } = 1;

}
