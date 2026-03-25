using System;

namespace CourseApiServices.Dtos.CategoryDtos;

public class CreateCategoryDto
{
      public int CategoryId { get; set; }

      public string CategoryName { get; set; } = null!;
}
