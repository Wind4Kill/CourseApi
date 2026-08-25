
using CourseApi.Application.DTOs.AuthorDtos;
using CourseApi.Application.DTOs.CategoryDtos;

namespace CourseApi.Application.DTOs.CourseDtos;

public class GetCourseByIdDto
{
      public int CourseId { get; set; }

      public required string CourseName { get; set; }

      public required string CourseDescription { get; set; }

      public decimal CoursePrice { get; set; }

      public double? CourseRating { get; set; }

      public GetAuthorDto Author { get; set; } = null!; 

      public List<ReviewDto>? Reviews { get; set; }

      public List<GetCategoryDto> Categories { get; set; } = null!;


}
