using System;
using CourseApi.Domain.HelpClasses;
using CourseApiDomain.Entities;
using CourseApiServices.Dtos.CourseDtos;

namespace CourseApiServices.Interfaces;

public interface ICourseService
{
      Task<List<GetCourseDto>> GetCourses(SortFilterOptions options, CancellationToken cancellationToken);

      Task<GetCourseByIdDto?> GetCourseById(int id, CancellationToken cancellationToken);
      Task<GetCourseByIdDto> CreateCourse(CreateCourseDto course, CancellationToken cancellationToken);
      Task RemoveCourse(int id, CancellationToken cancellationToken);

      Task UpdateCourse(int id, UpdateCourseDto updatedCourseDto, CancellationToken cancellationToken);
}
