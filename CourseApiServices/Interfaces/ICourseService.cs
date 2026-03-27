using System;
using CourseApiDomain.Entities;
using CourseApiServices.Dtos.CourseDtos;
using CourseApiServices.Interfaces.HelpClasses;

namespace CourseApiServices.Interfaces;

public interface ICourseService
{
      Task<List<GetCourseDto>> GetCourses(SortFilterOptions options);
      Task<(int, string)> CreateCourse(CreateCourseDto dto);

      public Task<GetCourseByIdDto?> GetCourseById(int id);
}
