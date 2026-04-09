using System;
using CourseApiServices.HelpClasses;

namespace CourseApiServices.Interfaces.HelpClasses;

public class SortFilterOptions
{
      public FilterOptions Filter { get; set; }
      public SortingOptions Sorting { get; set; } 

      public string? FilterValue { get; set; }

      public int PageNum { get; set; }

}
