using System;

namespace CourseApi.Api.FiltrationClasses;

public record Filtering(string? Filter,
string? Sorting,
string? FilterValue,
int? PageNum);

