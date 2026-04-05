using System;
using Microsoft.AspNetCore.Mvc;

namespace CourseApi.Enpoints;

public static class AuthorEndpoints
{
      public static void AddAuthorEndpoints(this WebApplication app)
      {
            var authorEndpoints = app.MapGroup("/authors").WithTags("Authors");

            authorEndpoints.MapGet("{**author}", ([FromRoute(Name="author")] string authors) => authors);
      }
}
