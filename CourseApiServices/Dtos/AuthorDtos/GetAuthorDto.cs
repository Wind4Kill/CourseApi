using System;
using CourseApiDomain.Entities;

namespace CourseApiServices.Dtos.AuthorDtos;

public class GetAuthorDto
{
      public int AuthorId { get; set; }

      public string Name { get; set; } = null!;

      public ICollection<Course> Courses { get; set; } = null!;
}
