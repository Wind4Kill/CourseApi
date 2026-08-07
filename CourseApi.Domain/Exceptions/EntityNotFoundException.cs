using System;

namespace CourseApi.Domain.Exceptions;

public class EntityNotFoundException:Exception
{
      public EntityNotFoundException(string message):base(message) {}
}
