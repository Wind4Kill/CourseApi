using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace CourseApi.Application.DTOs;

public class ReviewDto
{
      [MaxLength(200, ErrorMessage = "Review text must be maximum 200 characters in length")]
      public string? ReviewText { get; set; }

      [Required]
      public double ReviewRating { get; set; }
}
