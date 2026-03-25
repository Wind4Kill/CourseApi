using System;
using System.ComponentModel.DataAnnotations;

namespace CourseApiServices.Dtos.AuthorDtos;

public class CreateAuthorDto
{
      [Required]
      public int AuthorId { get; set; }
      [Required]
      public string AuthorName { get; set; } = null!;
}
