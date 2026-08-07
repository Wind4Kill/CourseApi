using System;
using CourseApi.Domain.HelpClasses;
using CourseApiDomain.Entities;
using CourseApiServices.Dtos.CourseDtos;

namespace CourseApiServices.Interfaces;

public interface ICourseService
{
      Task<List<GetCourseDto>> GetCourses(SortFilterOptions options);

      Task<GetCourseByIdDto?> GetCourseById(int id);
      Task<Course> CreateCourse(CreateCourseDto course);
      Task<int> RemoveCourse(int id);

      Task UpdateCourse(int id, UpdateCourseDto updatedCourseDto);
}
