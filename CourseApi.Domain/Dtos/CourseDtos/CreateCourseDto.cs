using System;
using System.ComponentModel.DataAnnotations;
using CourseApiServices.Dtos.AuthorDtos;
using CourseApiServices.Dtos.CategoryDtos;

namespace CourseApiServices.Dtos.CourseDtos;

public class CreateCourseDto
{
      public string CourseName { get; set; } = null!;

      public string CourseDescription { get; set; } = null!;

      public decimal CoursePrice { get; set; }

      public string Author { get; set; } = null!;
      public List<string> Categories { get; set; } = null!;
}
