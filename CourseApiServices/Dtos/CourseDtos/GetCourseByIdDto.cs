using System;
using CourseApiDomain.Entities;
using CourseApiServices.Dtos.AuthorDtos;
using CourseApiServices.Dtos.ReviewDtos;

namespace CourseApiServices.Dtos.CourseDtos;

public class GetCourseByIdDto
{
      public int CourseId { get; set; }

      public required string CourseName { get; set; }

      public required string CourseDescription { get; set; }

      public decimal CoursePrice { get; set; }

      public double? CourseRating { get; set; }

      public GetAuthorDto Author { get; set; } = null!; 

      public List<ReviewDto>? Reviews { get; set; }


}
