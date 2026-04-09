using System;
using System.Collections;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CourseApiDomain;
using CourseApiDomain.Entities;
using CourseApiServices.Dtos.AuthorDtos;
using CourseApiServices.Dtos.CourseDtos;
using CourseApiServices.Dtos.ReviewDtos;
using CourseApiServices.Interfaces.HelpClasses;
using Microsoft.EntityFrameworkCore;

namespace CourseApiServices.Interfaces.Repositories;



public class CourseRepository : ICourseRepository
{
      ApplicationContext _context;
      IMapper _mapper;

      public CourseRepository(ApplicationContext context, IMapper mapper)
      {
            _context = context;
            _mapper = mapper;
      }
      public async Task<GetCourseDto> AddCourse(Course addedCourse)
      {
            _context.Add(addedCourse);
            await _context.SaveChangesAsync();
            return new GetCourseDto
            {
                  CourseId = addedCourse.CourseId,
                  CourseName = addedCourse.CourseName,
                  CoursePrice = addedCourse.CourseDetails.CoursePrice,
            };
      }

      public async Task<List<GetCourseDto>> GetCourses(SortFilterOptions options)
      {
            List<GetCourseDto> courses = await _context.Courses.AsNoTracking().
            SortCourses(options.Sorting).
            FilterCourses(options.Filter, options.FilterValue).
            PaginatePage(options.PageNum).
            Select(c => new GetCourseDto
            {
                  CourseId = c.CourseId,
                  CourseName = c.CourseName,
                  CoursePrice = c.CourseDetails.CoursePrice,
                  CourseRating = ApplicationContext.GetCourseRating(c.CourseId)
            }).ToListAsync();

            return courses;
      }

      public async Task<GetCourseByIdDto?> GetCourseById(int id)
      {
            GetCourseByIdDto? course = await _context.Courses.AsNoTracking().
            Include(c => c.Authors).Include(c => c.Categories)
            .Include(c => c.Reviews).Select(c => new GetCourseByIdDto()
            {
                  CourseId = c!.CourseId,
                  CourseName = c.CourseName,
                  CoursePrice = c.CourseDetails.CoursePrice,
                  CourseDescription = c.CourseDetails.CourseDescription,
                  CourseRating = ApplicationContext.GetCourseRating(c.CourseId),
                  Reviews = c.Reviews!.Select(r => new ReviewDto
                  {
                        ReviewText = r.ReviewText,
                        ReviewRating = r.ReviewRating
                  }).ToList(),
                  Authors = c.Authors.Select(a => new GetAuthorDto
                  {
                        AuthorId = a.AuthorId,
                        Name = a.Name
                  }).ToList()
            }).
            SingleOrDefaultAsync(c => c.CourseId == id);
            return course;
      }

      public async Task<int> RemoveCourse(int id)
      {
            Course requestedCourse = _context.Courses.Single(c => c.CourseId == id);

            requestedCourse.IsDeleted = true;
            return await _context.SaveChangesAsync();

      }

      public async Task<int> UpdateCourse(int id, UpdateCourseDto updateCourseDto)
      {
            Course? requiredCourse = await _context.Courses.SingleOrDefaultAsync(c => c.CourseId == id);

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

                  requiredCourse.Authors = await DifferentiateEntity<Author>(updateCourseDto.Authors);
            }

            if (updateCourseDto.Categories.Any())
            {
                  requiredCourse.Categories = await DifferentiateEntity<Category>(updateCourseDto.Categories);
            }

            return await _context.SaveChangesAsync();
      }

      async Task<List<T>> DifferentiateEntity<T>(List<string> dtoNames) where T : class, IDifferentiateEntity, new()
      {
            List<T> existingValues = await _context.Set<T>().Where(t => dtoNames.Contains(t.Name)).ToListAsync();
            List<string> existingNames = new();
            foreach (var existingName in existingValues)
            {
                  existingNames.Add(existingName.Name);
            }

            List<string> newNames = dtoNames.Except(existingNames).ToList();

            List<T> newValues = new();

            foreach (string name in newNames)
            {
                  newValues.Add(new T() { Name = name });
            }

            return existingValues.Concat(newValues).ToList();

      }


}
