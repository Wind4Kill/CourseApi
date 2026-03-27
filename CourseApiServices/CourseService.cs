using System.Reflection;
using CourseApiDomain;
using CourseApiDomain.Entities;
using CourseApiServices.Dtos.CourseDtos;
using CourseApiServices.Interfaces;
using CourseApiServices.Interfaces.HelpClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CourseApiServices;



public class CourseService : ICourseService
{
      ApplicationContext _context;

      public CourseService(ApplicationContext context)
      {
            _context = context;
      }

      public async Task<(int, string)> CreateCourse(CreateCourseDto dto)
      {
            Course addedCourse = new Course()
            {
                  CourseId = dto.CourseId,
                  CourseName = dto.CourseName,
                  CourseDetails = new CourseDetails() { CourseDescription = dto.CourseDescription, CoursePrice = dto.CoursePrice },
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

            _context.Courses.Add(addedCourse);

            int affectedRows = await _context.SaveChangesAsync();
            return (affectedRows, addedCourse.CourseName);
      }

      public async Task<List<GetCourseDto>> GetCourses(SortFilterOptions options)
      {
            List<GetCourseDto> courses = await _context.Courses.AsNoTracking().
            SortCourses(options.Sorting).
            FilterCourses(options.Filter, options.FilterValue).
            PaginatePage(options.PageNum).Select(c => new GetCourseDto
            {
                  CourseId = c.CourseId,
                  CourseName = c.CourseName,
                  CoursePrice = c.CourseDetails.CoursePrice,
                  CourseRating = c.CourseRating
            }).
            ToListAsync();

            return courses;
      }

      public async Task<GetCourseByIdDto?> GetCourseById(int id)
      {
            GetCourseByIdDto? requiredCourse = await _context.Courses.Where(c => c.CourseId == id).AsNoTracking().
            Select(c => new GetCourseByIdDto()
            {
                  CourseId = c.CourseId,
                  CourseName = c.CourseName,
                  CoursePrice = c.CourseDetails.CoursePrice,
                  CourseDescription = c.CourseDetails.CourseDescription
            }).SingleOrDefaultAsync();
            return requiredCourse;
      }

      public async Task<int> RemoveCourse(int id)
      {
            Course? requiredCourse = _context.Courses.SingleOrDefault(c => c.CourseId == id);

            if (requiredCourse is null)
                  throw new ArgumentNullException("Entity with such Id was not found");

            requiredCourse.IsDeleted = true;

            return await _context.SaveChangesAsync();
      }

      public async Task<int> UpdateCourse(UpdateCourseDto updateCourseDto)
      {
            Course? requiredCourse = _context.Courses.Where(c => c.CourseId == updateCourseDto.CourseId)
            .Include(c => c.Authors)
            .Include(c => c.Categories).SingleOrDefault();

            if (requiredCourse is null)
            {
                  throw new Exception("Course hasn't been found");
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
            
            if(updateCourseDto.Categories.Any())
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
