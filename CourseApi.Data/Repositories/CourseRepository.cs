using System;
using System.Collections;
using CourseApiDomain;
using CourseApiDomain.Entities;
using CourseApiDomain.Views;
using CourseApiServices.Dtos.AuthorDtos;
using CourseApiServices.Dtos.CourseDtos;
using CourseApiServices.Dtos.ReviewDtos;
using CourseApiServices.Interfaces.HelpClasses;
using Microsoft.EntityFrameworkCore;

namespace CourseApiServices.Interfaces.Repositories;



public class CourseRepository : ICourseRepository
{
      readonly ApplicationContext _context;

      public CourseRepository(ApplicationContext context)
      {
            _context = context;
      }
      public async Task AddCourse(Course addedCourse)
      {
            _context.Add(addedCourse);
            await _context.SaveChangesAsync();

      }

      public IQueryable<Course> GetCourses()
      {
            IQueryable<Course> courses = _context.Courses.AsNoTracking();
            return courses;
      }

      public async Task<Course?> GetCourseById(int id)
      {
            Course? course = await _context.Courses.
            Include(c => c.Reviews).
            Include(c => c.Author).
            Include(c => c.Categories).
            FirstOrDefaultAsync(c => c.CourseId == id);
            
            CourseRating requestedRating = await _context.Ratings.Where(c => c.CourseId == id).FirstAsync();
            course?.AverageRating = requestedRating.AvgRating;
            return course;
      }

      public async Task<int> RemoveCourse(int id)
      {
            return await _context.Courses.Where(c => c.CourseId == id).
            ExecuteUpdateAsync(c => c.
            SetProperty(course => course.IsDeleted, course => true));
      }

      public async Task UpdateCourse()
      {
           await _context.SaveChangesAsync();
      }

      public async Task<Course?> FindCourseByName(string name)
      {
            Course? requiredCourse = await _context.Courses.SingleOrDefaultAsync(c => c.CourseName == name);
            return requiredCourse;
      }
}
