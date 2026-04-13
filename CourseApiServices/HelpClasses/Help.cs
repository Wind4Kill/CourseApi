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
            List<T> existingValues = existedValues.Where(t => dtoNames.Contains(t.Name)).ToList();
            List<string> existingNames = new();
            foreach (var existingName in existingValues)
            {
                  existingNames.Add(existingName.Name);
            }

            List<string> newNames = dtoNames.Except(existingNames).ToList();

            foreach (string name in newNames)
            {
                  newValues.Add(new T() { Name = name });
            }

            return existingValues.Concat(newValues).ToList();

      }
}
