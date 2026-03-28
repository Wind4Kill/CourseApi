using System;

namespace CourseApi;

public record Filtering(string? Filter="Default",
string? Sorting="Default",
string? FilterValue="",
int? PageNum=1);




