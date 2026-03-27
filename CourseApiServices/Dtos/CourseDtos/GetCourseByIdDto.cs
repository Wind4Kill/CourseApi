using System;

namespace CourseApiServices.Dtos.CourseDtos;

public class GetCourseByIdDto
{
      public int CourseId { get; set; }

      public string CourseName { get; set; } = null!;

      public string CourseDescription { get; set; } = null!;

      public decimal CoursePrice { get; set; }

      
}
