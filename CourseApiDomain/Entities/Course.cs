using System;

namespace CourseApiDomain.Entities;

public class Course
{
      public int CourseId { get; set; }

      public string CourseName { get; set; } = null!;

      public ICollection<Author> Authors { get; set; } = null!;

      public ICollection<Category> Categories { get; set; } = null!;

      public CourseDetails CourseDetails { get; set; } = null!;

      public double? CourseRating { get; private set; }

}
