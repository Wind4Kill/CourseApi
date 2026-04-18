using System;
using System.ComponentModel.DataAnnotations;
using CourseApiServices.Dtos.AuthorDtos;
using CourseApiServices.Dtos.CategoryDtos;

namespace CourseApiServices.Dtos.CourseDtos;

public class UpdateCourseDto
{
      [MaxLength(50)]
      public string? CourseName { get; set; } = null!;

      public decimal? CoursePrice { get; set; }

      [MaxLength(250)]
      public string? CourseDescription { get; set; } = null!;

      public string? Author{ get; set; }
      public List<string>? Categories { get; set; } = null!;


}
