using System;
using System.Security.Cryptography.X509Certificates;

namespace CourseApi.Application.DTOs;

public class ReviewDto
{
      public string? ReviewText { get; set; }

      public double ReviewRating { get; set; }
}
