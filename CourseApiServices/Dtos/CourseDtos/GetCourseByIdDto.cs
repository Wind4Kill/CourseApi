using System;
using CourseApiDomain.Entities;
using CourseApiServices.Dtos.AuthorDtos;

namespace CourseApiServices.Dtos.CourseDtos;

public class GetCourseByIdDto
{
      public int CourseId { get; set; }

      public string CourseName { get; set; } = null!;

      public string CourseDescription { get; set; } = null!;

      public decimal CoursePrice { get; set; }

     public ICollection<GetAuthorDto> Authors { get; set; } = null!;


}
