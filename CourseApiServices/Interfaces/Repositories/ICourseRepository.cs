using System;
using CourseApiDomain.Entities;
using CourseApiServices.Dtos.CourseDtos;
using CourseApiServices.HelpClasses;
using CourseApiServices.Interfaces.HelpClasses;

namespace CourseApiServices.Interfaces;

public interface ICourseRepository
{
      Task<GetCourseDto> AddCourse(Course addedCourse);

      Task<List<GetCourseDto>> GetCourses(SortFilterOptions options);

      Task<GetCourseByIdDto?> GetCourseById(int id);

      Task<int> RemoveCourse(int id);

      Task<int> UpdateCourse(int id, UpdateCourseDto updateCourseDto);
}
