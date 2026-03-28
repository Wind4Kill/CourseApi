using System;

namespace CourseApiDomain.Entities;

public class Course
{
      public int CourseId { get; set; }

      public string CourseName { get; set; } = null!;

      public required ICollection<Author> Authors { get; set; } 

      public required ICollection<Category> Categories { get; set; }
      public ICollection<Review>? Reviews { get; set; } 

      public required CourseDetails CourseDetails { get; set; }

      public double? _courseRating;

      public double? CourseRating => _courseRating;

      public bool IsDeleted { get; set; }


}
