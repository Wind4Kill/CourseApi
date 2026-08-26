using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseApi.Domain.Entities;

public class Course
{
      public int CourseId { get; set; }

      public string CourseName { get; set; } = null!;

      public Author Author { get; set; } = null!;

      public int AuthorId { get; set; }

      public ICollection<Category> Categories { get; set; } = null!;
      public ICollection<Review>? Reviews { get; set; }

      public double? AverageRating => Reviews?.Any() == true ? Reviews!.Average(r => r.ReviewRating) : 0.0;

      public required CourseDetails CourseDetails { get; set; }
      public bool IsDeleted { get; set; }

      public Guid Version { get; set; }


}
