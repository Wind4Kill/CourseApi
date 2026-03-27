using System;
using CourseApiDomain.Entities;
using CourseApiServices.Dtos.CourseDtos;
using CourseApiServices.Interfaces.HelpClasses;

namespace CourseApiServices.Interfaces;

public interface ICourseService
{
      Task<List<GetCourseDto>> GetCourses(SortFilterOptions options);
      Task<(int, string)> CreateCourse(CreateCourseDto dto);

      Task<GetCourseByIdDto?> GetCourseById(int id);

      Task<int> RemoveCourse(int id);

      Task<int> UpdateCourse(UpdateCourseDto updatedCourseDto);
}
