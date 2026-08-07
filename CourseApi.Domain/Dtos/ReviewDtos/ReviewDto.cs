using System;
using System.Security.Cryptography.X509Certificates;

namespace CourseApiServices.Dtos.ReviewDtos;

public class ReviewDto
{
      public string? ReviewText { get; set; }

      public double ReviewRating { get; set; }
}
