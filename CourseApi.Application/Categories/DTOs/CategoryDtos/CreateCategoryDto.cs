using System;
using System.ComponentModel.DataAnnotations;

namespace CourseApi.Application.DTOs.CategoryDtos;

public class CreateCategoryDto
{
      [Required]
      public string CategoryName { get; set; } = null!;
}
