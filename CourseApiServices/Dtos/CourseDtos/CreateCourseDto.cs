using System;
using System.ComponentModel.DataAnnotations;
using CourseApiServices.Dtos.AuthorDtos;
using CourseApiServices.Dtos.CategoryDtos;

namespace CourseApiServices.Dtos.CourseDtos;

public class CreateCourseDto
{
      [Required]
      public string CourseName { get; set; } = null!;

[Required]
      public string CourseDescription { get; set; } = null!;

[Required]
      public decimal CoursePrice { get; set; }

[Required]
      public ICollection<CreateAuthorDto> Authors { get; set; } = null!;
[Required]
      public ICollection<CreateCategoryDto> Categories { get; set; } = null!;
}
