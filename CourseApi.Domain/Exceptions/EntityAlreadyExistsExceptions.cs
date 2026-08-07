using System;

namespace CourseApi.Domain.Exceptions;

public class EntityAlreadyExistsExceptions:Exception
{
      public EntityAlreadyExistsExceptions(string message):base(message){}
}
