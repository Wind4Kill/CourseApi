using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseApiDomain.Entities;

public class Course
{
      public int CourseId { get; set; }

      public string CourseName { get; set; } = null!;

      public required ICollection<Author> Authors { get; set; }

      public required ICollection<Category> Categories { get; set; }
      public ICollection<Review>? Reviews { get; set; }

      public required CourseDetails CourseDetails { get; set; }

      public bool IsDeleted { get; set; }


}
