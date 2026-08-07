using System;
using System.Collections;
using CourseApiDomain;
using CourseApiDomain.Entities;
using CourseApiDomain.Views;

using Microsoft.EntityFrameworkCore;

namespace CourseApiServices.Interfaces.Repositories;



public class CourseRepository : ICourseRepository
{
      readonly ApplicationContext _context;

      public CourseRepository(ApplicationContext context)
      {
            _context = context;
      }
      public async Task<Course> AddCourse(Course addedCourse, CancellationToken cancellationToken)
      {
            _context.Add(addedCourse);
            await _context.SaveChangesAsync(cancellationToken);
            return addedCourse;
      }

      public IQueryable<Course> GetCourses()
      {
            IQueryable<Course> courses = _context.Courses.AsNoTracking();
            return courses;
      }

      public async Task<Course?> GetCourseById(int id, CancellationToken cancellationToken)
      {
            Course? course = await _context.Courses.
            Include(c => c.Reviews).
            Include(c => c.Author).
            Include(c => c.Categories).
            FirstOrDefaultAsync(c => c.CourseId == id, cancellationToken);
            
            CourseRating requestedRating = await _context.Ratings.Where(c => c.CourseId == id).FirstAsync(cancellationToken);
            course?.AverageRating = requestedRating.AvgRating;
            return course;
      }

      public async Task RemoveCourse(Course course, CancellationToken cancellationToken)
      {
            course.IsDeleted = true;
            await _context.SaveChangesAsync(cancellationToken);
      }

      public async Task UpdateCourse(CancellationToken cancellationToken)
      {
           await _context.SaveChangesAsync(cancellationToken);
      }

      public async Task<Course?> FindCourseByName(string name, CancellationToken cancellationToken)
      {
            Course? requiredCourse = await _context.Courses
            .SingleOrDefaultAsync(c => c.CourseName == name, cancellationToken);
            
            return requiredCourse;
      }
}
