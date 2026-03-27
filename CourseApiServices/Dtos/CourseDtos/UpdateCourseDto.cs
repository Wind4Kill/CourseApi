using System;
using System.ComponentModel.DataAnnotations;
using CourseApiServices.Dtos.AuthorDtos;
using CourseApiServices.Dtos.CategoryDtos;

namespace CourseApiServices.Dtos.CourseDtos;

public class UpdateCourseDto
{
      public string CourseName { get; set; } = null!;

      public decimal CoursePrice { get; set; }

      public string CourseDescription { get; set; } = null!;

      public List<string> Authors { get; set; } = null!;
      public List<string> Categories { get; set; } = null!;


}
