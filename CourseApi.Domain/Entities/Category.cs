
using CourseApi.Domain.Interfaces;

namespace CourseApi.Domain.Entities;

public class Category:IDifferentiateEntity
{
      public int CategoryId { get; set; }

      public string Name { get; set; } = null!;

      public ICollection<Course>? Courses { get; set; }
      
}
