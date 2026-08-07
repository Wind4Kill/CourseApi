using System;
using CourseApiDomain.Entities;
using CourseApiServices.Dtos.CourseDtos;

namespace CourseApiServices.Interfaces;

public interface ICourseRepository
{
      Task AddCourse(Course addedCourse);

      IQueryable<Course> GetCourses();

      Task<Course?> GetCourseById(int id);

      Task<int> RemoveCourse(Course course);

      Task UpdateCourse();

      Task<Course?> FindCourseByName(string name);


}
