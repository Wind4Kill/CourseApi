using System;

namespace CourseApi.Domain.Entities;

public class Review
{
      public int ReviewId { get; set; }

      public string? ReviewText { get; set; }

      public double ReviewRating { get; set; }

      public int CourseId { get; set; }

}
