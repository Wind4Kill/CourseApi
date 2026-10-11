using System;

namespace CourseApi.Domain.Entities;

public class Review
{
      public int ReviewId { get; set; }

      public User User { get; set; } = null!;

      public string UserId { get; set; } = null!;

      public string? ReviewText { get; set; }

      public double ReviewRating { get; set; }

      public int CourseId { get; set; }

}
