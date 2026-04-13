using System;

namespace CourseApiServices.Interfaces.HelpClasses;

public class EntityNotFoundException:Exception
{
      public EntityNotFoundException(string message):base(message) {}
}
