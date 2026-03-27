using System;
using CourseApiDomain.Entities;
using CourseApiServices.Dtos.CourseDtos;
using CourseApiServices.HelpClasses;
using CourseApiServices.Interfaces.HelpClasses;

namespace CourseApiServices.Interfaces;

public interface ICourseRepository
{
      Task<Course> AddCourse(Course addedCourse);

      Task<IEnumerable<Course>> GetCourses(SortFilterOptions options);

      Task<Course?> GetCourseById(int id);

      Task<int> RemoveCourse(int id);

      Task<int> UpdateCourse(UpdateCourseDto updateCourseDto);
}
