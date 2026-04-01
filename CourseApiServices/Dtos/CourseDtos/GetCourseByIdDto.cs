using System;
using AutoMapper;
using CourseApiDomain.Entities;
using CourseApiServices.Dtos.AuthorDtos;
using CourseApiServices.Dtos.ReviewDtos;

namespace CourseApiServices.Dtos.CourseDtos;

[AutoMap(typeof(Course))]
public class GetCourseByIdDto
{
      public int CourseId { get; set; }

      public required string CourseName { get; set; }

      public required string CourseDescription { get; set; }

      public decimal CoursePrice { get; set; }

      public double? CourseRating { get; set; }

      public required ICollection<GetAuthorDto> Authors { get; set; }

      public ICollection<ReviewDto>? Reviews { get; set; }


}
