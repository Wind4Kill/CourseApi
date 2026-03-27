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

      public async Task<Course> CreateCourse(CreateCourseDto dto)
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
                  Authors = requestedCourse.Authors.Select(a => new GetAuthorDto
                  {
                        AuthorId = a.AuthorId,
                        Name = a.Name
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

      public async Task<int> UpdateCourse(int id, UpdateCourseDto updateCourseDto)
      {
           return await _repository.UpdateCourse(id, updateCourseDto);
      }
}
