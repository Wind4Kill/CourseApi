using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseApiDomain.Entities;

public class Course
{
      public int CourseId { get; set; }

      public string CourseName { get; set; } = null!;

      public Author Author { get; set; } = null!;

      public int AuthorId { get; set; }

      public ICollection<Category> Categories { get; set; } = null!;
      public ICollection<Review>? Reviews { get; set; }

      public double? AverageRating { get; set; }

      public required CourseDetails CourseDetails { get; set; }
      public bool IsDeleted { get; set; }

      public Guid Version { get; set; }


}
