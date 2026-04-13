using System.Reflection;
using CourseApiDomain;
using CourseApiDomain.Entities;
using CourseApiServices.Dtos.AuthorDtos;
using CourseApiServices.Dtos.CourseDtos;
using CourseApiServices.Dtos.ReviewDtos;
using CourseApiServices.HelpClasses;
using CourseApiServices.Interfaces;
using CourseApiServices.Interfaces.HelpClasses;
using CourseApiServices.Interfaces.Repositories;
using CourseApiServices.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CourseApiServices;



public class CourseService : ICourseService
{
      readonly IAuthorRepository _authorRepository;

      readonly ICategoryRepository _categoryRepository;
      readonly ICourseRepository _courseRepository;

      public CourseService(ICourseRepository courseRepository, IAuthorRepository authorRepository, ICategoryRepository categoryRepository)
      {
            _courseRepository = courseRepository;
            _authorRepository = authorRepository;
            _categoryRepository = categoryRepository;
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

            List<string> dtoAuthorsNames = dto.Authors.Select(a=>a.AuthorName).ToList();

            List<Author>? existedAuthors = await _authorRepository.GetAuthorsByNames(dtoAuthorsNames);

            List<string> dtoCategoriesNames = dto.Categories.Select(c => c.CategoryName).ToList();

            List<Category>? existedCategories = await _categoryRepository.GetCategoriesByNames(dtoCategoriesNames);

            addedCourse.Authors = await Help.DifferentiateEntity(dtoAuthorsNames, existedAuthors);

            await _courseRepository.AddCourse(addedCourse);

            return addedCourse;
      }

      public async Task<List<GetCourseDto>> GetCourses(SortFilterOptions options)
      {
            IQueryable<Course> courses = _courseRepository.GetCourses();

            List<GetCourseDto> mappedCourses = await courses.AsNoTracking().
                        SortCourses(options.Sorting).
                        FilterCourses(options.Filter, options.FilterValue).
                        PaginatePage(options.PageNum).
                        Select(c => new GetCourseDto
                        {
                              CourseId = c.CourseId,
                              CourseName = c.CourseName,
                              CoursePrice = c.CourseDetails.CoursePrice,
                              CourseRating = CourseFunctions.GetCourseRating(c.CourseId)
                        }).ToListAsync();

            return mappedCourses;
      }

      public async Task<GetCourseByIdDto?> GetCourseById(int id)
      {
            Course? course = await _courseRepository.GetCourseById(id);

            if (course is null)
            {
                  throw new EntityNotFoundException("Course hasn't been founded");
            }

            GetCourseByIdDto mappedCourse = new GetCourseByIdDto()
            {
                  CourseId = course.CourseId,
                  CourseName = course.CourseName,
                  CoursePrice = course.CourseDetails.CoursePrice,
                  CourseDescription = course.CourseDetails.CourseDescription,
                  CourseRating = CourseFunctions.GetCourseRating(course.CourseId),
                  Authors = course.Authors.Select(a => new GetAuthorDto()
                  {
                        AuthorId = a.AuthorId,
                        Name = a.Name
                  }).ToList(),

            };

            if (course.Reviews is not null)
            {
                  mappedCourse.Reviews = course.Reviews.Select(r => new ReviewDto()
                  {
                        ReviewText = r.ReviewText,
                        ReviewRating = r.ReviewRating
                  }).ToList();
            }

            return mappedCourse;

      }

      public async Task<int> RemoveCourse(int id)
      {
            return await _courseRepository.RemoveCourse(id);
      }

      public async Task<int> UpdateCourse(int id, UpdateCourseDto updateCourseDto)
      {
            Course? requiredCourse = await _courseRepository.GetCourseById(id);

            if (requiredCourse is null)
            {
                  throw new InvalidOperationException("Course hasn't been found");
            }

            if (!updateCourseDto.CourseName.Equals(requiredCourse.CourseName) && !string.IsNullOrEmpty(updateCourseDto.CourseName))
            {
                  requiredCourse.CourseName = updateCourseDto.CourseName;
            }

            if (!updateCourseDto.CourseDescription.Equals(requiredCourse.CourseDetails.CourseDescription) && !string.IsNullOrEmpty(updateCourseDto.CourseDescription))
            {
                  requiredCourse.CourseDetails.CourseDescription = updateCourseDto.CourseDescription;
            }

            if (!updateCourseDto.CoursePrice.Equals(requiredCourse.CourseDetails.CoursePrice) && updateCourseDto.CoursePrice is not 0)
            {
                  requiredCourse.CourseDetails.CoursePrice = updateCourseDto.CoursePrice;
            }

            if (updateCourseDto.Authors.Any())
            {

                  requiredCourse.Authors = await Help.DifferentiateEntity<Author>(updateCourseDto.Authors, requiredCourse.Authors as List<Author>);
            }

            if (updateCourseDto.Categories.Any())
            {
                  requiredCourse.Categories = await Help.DifferentiateEntity<Category>(updateCourseDto.Categories, requiredCourse.Categories as List<Category>);
            }

            return await _courseRepository.UpdateCourse();
      }
}
