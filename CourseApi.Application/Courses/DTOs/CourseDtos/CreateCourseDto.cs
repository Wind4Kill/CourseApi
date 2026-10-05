using System;
using System.ComponentModel.DataAnnotations;

namespace CourseApi.Application.DTOs.CourseDtos;

public class CreateCourseDto
{
      public string CourseName { get; set; } = null!;

      public string CourseDescription { get; set; } = null!;

      public decimal CoursePrice { get; set; }

      public string Author { get; set; } = null!;
      public List<string> Categories { get; set; } = null!;
}
