
using CourseApi.Domain.Interfaces;

namespace CourseApi.Domain.Entities;

public class Author:IDifferentiateEntity
{
      public int AuthorId { get; set; }

      public string Name { get; set; } = null!;

      public bool IsDeleted { get; set; }

      public ICollection<Course>? Courses { get; set; }
}
