using System;

namespace CourseApiDomain.Entities;

public class Review
{
      public int ReviewId { get; set; }

      public string? ReviewText { get; set; }

      public double ReviewRating { get; set; }

      public int CourseId { get; set; }

}
