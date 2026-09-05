using System;
using System.ComponentModel.DataAnnotations;

namespace CourseApi.Application.DTOs.AuthorDtos;

public class CreateAuthorDto
{
      [Required]
      public string AuthorName { get; set; } = null!;
}
