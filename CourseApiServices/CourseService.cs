using System.Reflection;
using CourseApiDomain;
using CourseApiDomain.Entities;
using CourseApiServices.Dtos.AuthorDtos;
using CourseApiServices.Dtos.CourseDtos;
using CourseApiServices.Dtos.ReviewDtos;
using CourseApiServices.Interfaces;
using CourseApiServices.Interfaces.HelpClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CourseApiServices;



public class CourseService : ICourseService
{
      ICourseRepository _repository;

      public CourseService(ICourseRepository repository)
      {
            _repository = repository;
      }

      public async Task<GetCourseDto> CreateCourse(CreateCourseDto dto)
      {
            Course addedCourse = new Course()
            {
                  CourseName = dto.CourseName,
                  CourseDetails = new CourseDetails()
                  {
                        CourseDescription = dto.CourseDescription,
                        CoursePrice = dto.CoursePrice
                  },
                  Authors = dto.Authors.
                  Select(dto => new Author()
                  {
                        Name = dto.AuthorName
                  }).ToList(),
                  Categories = dto.Categories.
                  Select(dto => new Category()
                  {
                        Name = dto.CategoryName
                  }).ToList()

            };

            return await _repository.AddCourse(addedCourse);

            
      }

      public async Task<List<GetCourseDto>> GetCourses(SortFilterOptions options)
      {
            return await _repository.GetCourses(options);
      }

      public async Task<GetCourseByIdDto?> GetCourseById(int id)
      {
            GetCourseByIdDto? requestedCourse = await _repository.GetCourseById(id);

            return requestedCourse;
            
      }

      public async Task<int> RemoveCourse(int id)
      {
                  return await _repository.RemoveCourse(id);
      }

      public async Task<int> UpdateCourse(int id, UpdateCourseDto updateCourseDto)
      {
           return await _repository.UpdateCourse(id, updateCourseDto);
      }
}
