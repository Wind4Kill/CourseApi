using System;

namespace CourseApiDomain.Entities;

public class Author
{
      public int AuthorId { get; set; }

      public string AuthorName { get; set; } = null!;

      public ICollection<Course> Books { get; set; } = null!;
}
