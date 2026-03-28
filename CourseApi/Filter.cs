using System;

namespace CourseApi;

public record Filtering
{
      public string? Filter { get; init; } = "Default";
      public string? Sorting { get; init; } = "Default";
      public string? FilterValue { get; init; } = "";
      public int? PageNum { get; init; } = 1;
}



