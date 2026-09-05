
using CourseApi.Application.Filtration.HelpClasses;
using CourseApi.Domain.Entities;

namespace CourseApi.Application.Interfaces.Repositories;

public interface ICourseRepository
{
      Task<Course> AddCourse(Course addedCourse, CancellationToken cancellationToken);

      Task<List<Course>> GetCourses(SortFilterOptions filterOptions, CancellationToken cancellationToken);

      Task<Course?> GetCourseById(int id, CancellationToken cancellationToken);

      Task RemoveCourse(Course course, CancellationToken cancellationToken);

      Task UpdateCourse(CancellationToken cancellationToken);

      Task<Course?> FindCourseByName(string name, CancellationToken cancellationToken);


}
