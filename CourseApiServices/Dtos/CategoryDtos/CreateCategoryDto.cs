using System;
using System.ComponentModel.DataAnnotations;

namespace CourseApiServices.Dtos.CategoryDtos;

public class CreateCategoryDto
{
      [Required]
      public string CategoryName { get; set; } = null!;
}
