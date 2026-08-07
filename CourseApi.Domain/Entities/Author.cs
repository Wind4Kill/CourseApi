using System;

namespace CourseApiDomain.Entities;

public class Author:IDifferentiateEntity
{
      public int AuthorId { get; set; }

      public string Name { get; set; } = null!;

      public bool IsDeleted { get; set; }

      public ICollection<Course>? Courses { get; set; }
}
