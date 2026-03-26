using System;

namespace CourseApiServices.Dtos.CourseDtos;

public struct GetCourseDto
{
      public int CourseId { get; set; }

      public string CourseName { get; set; }

      public double? CourseRating { get; set; }

      public decimal CoursePrice { get; set; }

}
