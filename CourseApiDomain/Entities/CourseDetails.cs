using System;

namespace CourseApiDomain.Entities;

public class CourseDetails
{
      public decimal CoursePrice { get; set; }

      public string CourseDescription { get; set; } = null!;
}
