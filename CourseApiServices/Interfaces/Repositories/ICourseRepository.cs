using System;
using CourseApiDomain.Entities;
using CourseApiServices.Dtos.CourseDtos;
using CourseApiServices.HelpClasses;
using CourseApiServices.Interfaces.HelpClasses;

namespace CourseApiServices.Interfaces;

public interface ICourseRepository
{
      Task AddCourse(Course addedCourse);

      IQueryable<Course> GetCourses();

      Task<Course?> GetCourseById(int id);

      Task<int> RemoveCourse(int id);

      Task<int> UpdateCourse();

      
}
