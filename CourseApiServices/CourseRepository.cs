using System;
using System.Collections;
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
            IQueryable<Course> courses = _context.Courses;

            return courses;
      }

      public async Task<Course?> GetCourseById(int id)
      {
            Course? course = await _context.Courses.Include(c => c.Reviews).Include(c => c.Authors).
            FirstOrDefaultAsync(c => c.CourseId == id);
            return course;
      }

      public async Task<int> RemoveCourse(int id)
      {
            Course requestedCourse = await _context.Courses.SingleAsync(c => c.CourseId == id);

            requestedCourse.IsDeleted = true;
            return await _context.SaveChangesAsync();

      }

      public async Task<int> UpdateCourse()
      {
            return await _context.SaveChangesAsync();
      }


}
