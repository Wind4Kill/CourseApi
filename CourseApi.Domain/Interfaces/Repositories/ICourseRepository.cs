using System;
using CourseApiDomain.Entities;
using CourseApiServices.Dtos.CourseDtos;

namespace CourseApiServices.Interfaces;

public interface ICourseRepository
{
      Task<Course> AddCourse(Course addedCourse, CancellationToken cancellationToken);

      IQueryable<Course> GetCourses();

      Task<Course?> GetCourseById(int id, CancellationToken cancellationToken);

      Task RemoveCourse(Course course, CancellationToken cancellationToken);

      Task UpdateCourse(CancellationToken cancellationToken);

      Task<Course?> FindCourseByName(string name, CancellationToken cancellationToken);


}
