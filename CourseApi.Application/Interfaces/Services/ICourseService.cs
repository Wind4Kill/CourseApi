
using CourseApi.Application.DTOs;
using CourseApi.Application.DTOs.CourseDtos;
using CourseApi.Application.Filtration.HelpClasses;
using CourseApi.Application.Reviews;

namespace CourseApi.Application.Interfaces.Services;

public interface ICourseService
{
      Task<List<GetCourseDto>> GetCourses(SortFilterOptions options, CancellationToken cancellationToken);

      Task<GetCourseByIdDto?> GetCourseById(int id, CancellationToken cancellationToken);
      Task<GetCourseByIdDto> CreateCourse(CreateCourseDto course, CancellationToken cancellationToken);
      Task RemoveCourse(int id, CancellationToken cancellationToken);
      Task<GetReviewDto> AddReviewToCourse(int courseId, ReviewDto reviewDto, CancellationToken cancellationToken);
      Task UpdateCourse(int id, UpdateCourseDto updatedCourseDto, CancellationToken cancellationToken);
}
