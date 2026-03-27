using System;
using CourseApiDomain.Entities;
using CourseApiServices.Dtos.CourseDtos;

namespace CourseApiServices.Dtos.AuthorDtos;

public class GetAuthorDto
{
      public int AuthorId { get; set; }

      public string Name { get; set; } = null!;

      public ICollection<GetCourseDto> Courses { get; set; } = null!;
}
