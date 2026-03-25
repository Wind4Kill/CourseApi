using System;
using System.ComponentModel.DataAnnotations;
using CourseApiServices.Dtos.AuthorDtos;
using CourseApiServices.Dtos.CategoryDtos;

namespace CourseApiServices.Dtos.CourseDtos;

public class CreateCourseDto
{
      [Required]
      public int CourseId { get; set; }

      [Required]
      public string CourseName { get; set; } = null!;

      public string CourseDescription { get; set; } = null!;

      public decimal CoursePrice { get; set; }

      public ICollection<CreateAuthorDto> Authors { get; set; } = null!;

      public ICollection<CreateCategoryDto> Categories { get; set; } = null!;
}
