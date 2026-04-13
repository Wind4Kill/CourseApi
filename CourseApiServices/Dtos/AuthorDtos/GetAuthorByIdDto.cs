using System;
using CourseApiServices.Dtos.CourseDtos;

namespace CourseApiServices.Dtos.AuthorDtos;

public class GetAuthorByIdDto
{
      public int AuthorId { get; set; }

      public string AuthorName { get; set; } = null!;

      public ICollection<GetCourseByIdDto> Courses { get; set; } = null!;
}
