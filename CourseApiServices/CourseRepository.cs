using System;
using System.Collections;
using CourseApiDomain;
using CourseApiDomain.Entities;
using CourseApiServices.Dtos.CourseDtos;
using CourseApiServices.Interfaces.HelpClasses;
using Microsoft.EntityFrameworkCore;

namespace CourseApiServices.Interfaces.Repositories;



public class CourseRepository : ICourseRepository
{
      ApplicationContext _context;

      public CourseRepository(ApplicationContext context)
      {
            _context = context;
      }
      public async Task<Course> AddCourse(Course addedCourse)
      {
            _context.Add(addedCourse);
            await _context.SaveChangesAsync();
            return addedCourse;
      }

      public async Task<IEnumerable<Course>> GetCourses(SortFilterOptions options)
      {
            List<Course> courses = await _context.Courses.AsNoTracking().
            SortCourses(options.Sorting).
            FilterCourses(options.Filter, options.FilterValue).
            PaginatePage(options.PageNum).ToListAsync();

            return courses;
      }

      public async Task<Course?> GetCourseById(int id)
      {
            Course? course = _context.Courses.AsNoTracking().
            Include(c => c.Authors).Include(c=>c.Categories).
            SingleOrDefault(c => c.CourseId == id);
            return course;
      }

      public async Task<int> RemoveCourse(int id)
      {
            Course requestedCourse = _context.Courses.Single(c => c.CourseId == id);
            if (requestedCourse is null)
            {
                  throw new Exception("Requested course wasn't found");
            }

            requestedCourse.IsDeleted = true;
            return await _context.SaveChangesAsync();

      }

      public async Task<int> UpdateCourse(UpdateCourseDto updateCourseDto)
      {
            Course? requiredCourse = await _context.Courses.SingleOrDefaultAsync(c => c.CourseId == updateCourseDto.CourseId);

            if (requiredCourse is null)
            {
                  throw new ArgumentNullException("Course hasn't been found");
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
