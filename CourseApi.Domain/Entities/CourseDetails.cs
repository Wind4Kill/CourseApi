using System;

namespace CourseApi.Domain.Entities;

public class CourseDetails
{
      public decimal CoursePrice { get; set; }

      public string CourseDescription { get; set; } = null!;
}
