
using CourseApi.Application.Filtration.HelpClasses;
using CourseApi.Domain.Entities;

namespace CourseApi.Application.Interfaces.Repositories;

public interface ICourseRepository
{
      Course AddCourse(Course addedCourse);

      Task<List<Course>> GetCourses(SortFilterOptions filterOptions, CancellationToken cancellationToken);

      Task<Course?> GetCourseById(int id, CancellationToken cancellationToken);

      void RemoveCourse(Course course);
      Task<Course?> FindCourseByName(string name, CancellationToken cancellationToken);


}
