using System.Reflection;
using CourseApiDomain;
using CourseApiDomain.Entities;
using CourseApiServices.Dtos.AuthorDtos;
using CourseApiServices.Dtos.CourseDtos;
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

      public async Task<(int, string)> CreateCourse(CreateCourseDto dto)
      {
            Course addedCourse = new Course()
            {
                  CourseId = dto.CourseId,
                  CourseName = dto.CourseName,
                  CourseDetails = new CourseDetails()
                  {
                        CourseDescription = dto.CourseDescription,
                        CoursePrice = dto.CoursePrice
                  },
                  Authors = dto.Authors.
                  Select(dto => new Author()
                  {
                        AuthorId = dto.AuthorId,
                        Name = dto.AuthorName
                  }).ToList(),
                  Categories = dto.Categories.
                  Select(dto => new Category()
                  {
                        CategoryId = dto.CategoryId,
                        Name = dto.CategoryName
                  }).ToList()

            };

            int affectedRows = await _repository.AddCourse(addedCourse);

            return (affectedRows, addedCourse.CourseName);
      }

      public async Task<List<GetCourseDto>> GetCourses(SortFilterOptions options)
      {
            List<GetCourseDto> courses = (await _repository.GetCourses(options)).Select(c => new GetCourseDto
            {
                  CourseId = c.CourseId,
                  CourseName = c.CourseName,
                  CoursePrice = c.CourseDetails.CoursePrice,
                  CourseRating = c.CourseRating
            }).ToList();

            return courses;
      }

      public async Task<GetCourseByIdDto?> GetCourseById(int id)
      {
            Course? requestedCourse = await _repository.GetCourseById(id);
            if (requestedCourse is null)
            {
                  throw new Exception("Required course wasn't found");
            }
            return new GetCourseByIdDto()
            {
                  CourseId = requestedCourse!.CourseId,
                  CourseName = requestedCourse.CourseName,
                  CoursePrice = requestedCourse.CourseDetails.CoursePrice,
                  CourseDescription = requestedCourse.CourseDetails.CourseDescription,
                  Authors = requestedCourse.Authors.Select(a => new CreateAuthorDto
                  {
                        AuthorId = a.AuthorId,
                        AuthorName = a.Name
                  }).ToList()
            };
      }

      public async Task<int> RemoveCourse(int id)
      {
            try
            {
                  return await _repository.RemoveCourse(id);

            }
            catch(Exception)
            {
                  throw;
            }
      }

      public async Task<int> UpdateCourse(UpdateCourseDto updateCourseDto)
      {
           return await _repository.UpdateCourse(updateCourseDto);
      }
}
