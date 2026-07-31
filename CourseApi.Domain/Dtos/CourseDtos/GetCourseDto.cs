using System;
using CourseApiDomain.Entities;

namespace CourseApiServices.Dtos.CourseDtos;

public class GetCourseDto
{
      public int CourseId { get; set; }

      public string CourseName { get; set; } = null!;

      public double? CourseRating { get; set; }

      public decimal CoursePrice { get; set; }

}
