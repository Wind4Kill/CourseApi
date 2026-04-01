using System;
using CourseApiDomain.Entities;
using CourseApiServices.Dtos.CourseDtos;
using CourseApiServices.Interfaces.HelpClasses;

namespace CourseApiServices.Interfaces;

public interface ICourseService
{
      Task<List<GetCourseDto>> GetCourses(SortFilterOptions options);

      Task<GetCourseByIdDto?> GetCourseById(int id);
      Task<GetCourseDto> CreateCourse(CreateCourseDto dto);
      Task<int> RemoveCourse(int id);

      Task<int> UpdateCourse(int id, UpdateCourseDto updatedCourseDto);
}
