using System;

namespace CourseApi;

public record Filtering(string? Filter,
string? Sorting,
string? FilterValue,
int? PageNum);

