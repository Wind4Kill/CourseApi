using CourseApiDomain;
using CourseApiDomain.Entities;
using CourseApiServices.Dtos.CourseDtos;
using CourseApiServices.Interfaces;
using CourseApiServices.Interfaces.HelpClasses;
using Microsoft.EntityFrameworkCore;

namespace CourseApiServices;



public class CourseService : ICourseService
{
      ApplicationContext _context;

      public CourseService(ApplicationContext context)
      {
            _context = context;
      }

      public async Task<int> CreateCourse(CreateCourseDto dto)
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
                        AuthorName = dto.AuthorName
                  }).ToList(),
                  Categories = dto.Categories.
                  Select(dto => new Category()
                  {
                        CategoryId = dto.CategoryId,
                        CategoryName = dto.CategoryName
                  }).ToList()

            };

            _context.Courses.Add(addedCourse);

           return await _context.SaveChangesAsync();
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
}
