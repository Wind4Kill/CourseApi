using System;
using System.ComponentModel.DataAnnotations;

namespace CourseApiServices.Dtos.AuthorDtos;

public class CreateAuthorDto
{
      [Required]
      public string AuthorName { get; set; } = null!;
}
