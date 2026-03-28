using System;

namespace CourseApiDomain.Entities;

public class Category:IDifferentiateEntity
{
      public int CategoryId { get; set; }

      public string Name { get; set; } = null!;

      public ICollection<Course>? Courses { get; set; }
      
}
