using CourseApi.Application.DTOs.CourseDtos;

namespace CourseApi.Application.DTOs.AuthorDtos;

public class GetAuthorDto
{
      public int AuthorId { get; set; }

      public string Name { get; set; } = null!;

      public List<GetCourseDto> Courses { get; set; } = null!;
}
