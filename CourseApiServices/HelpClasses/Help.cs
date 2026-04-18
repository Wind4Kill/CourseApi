using System;
using CourseApiDomain.Entities;

namespace CourseApiServices.HelpClasses;

public static class Help
{
      public static async Task<List<T>> DifferentiateEntity<T>(List<string> dtoNames, List<T>? existedValues) where T : class, IDifferentiateEntity, new()
      {
            
            List<T> newValues = new();
            
            if (existedValues is null)
            {
                  foreach (string name in dtoNames)
                  {
                        newValues.Add(new T(){Name=name});
                  }

                  return newValues;
            }
            string[] existedNames = existedValues.Select(c => c.Name).ToArray();

            string[] newNames = dtoNames.Except(existedNames).ToArray();
            
            foreach (var name in newNames)
            {
                  newValues.Add(new T(){Name=name});
            }

            return existedValues.Concat(newValues).ToList();

      }
}
