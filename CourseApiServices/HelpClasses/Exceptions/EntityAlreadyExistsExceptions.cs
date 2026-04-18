using System;

namespace CourseApiServices.HelpClasses.Exceptions;

public class EntityAlreadyExistsExceptions:Exception
{
      public EntityAlreadyExistsExceptions(string message):base(message){}
}
