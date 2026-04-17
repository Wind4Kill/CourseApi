using System;
using System.ComponentModel.DataAnnotations;
using CourseApiServices.Dtos.AuthorDtos;
using CourseApiServices.Dtos.CategoryDtos;

namespace CourseApiServices.Dtos.CourseDtos;

public class CreateCourseDto
{
      [Required]
      [MaxLength(50)]
      public string CourseName { get; set; } = null!;

      [Required]
      [MaxLength(250)]
      public string CourseDescription { get; set; } = null!;

      [Required]
      public decimal CoursePrice { get; set; }

      [Required]
      public string Author { get; set; } = null!;
      [Required]
      public List<CreateCategoryDto> Categories { get; set; } = null!;
}
