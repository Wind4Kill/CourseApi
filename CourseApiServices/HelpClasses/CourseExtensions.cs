using System;
using CourseApiDomain.Entities;
using CourseApiServices.HelpClasses;

namespace CourseApiServices.Interfaces.HelpClasses;

public static class CourseExtensions
{
      public static IQueryable<Course> SortCourses(this IQueryable<Course> courses,
       SortingOptions options)
      {
            return options switch
            {
                  SortingOptions.ByName => courses.OrderBy(c => c.CourseName),
                  SortingOptions.ByPrice => courses.OrderBy(c => c.CourseDetails.CoursePrice),
                  _ => courses.OrderBy(c => c.CourseId)
            };
      }

      public static IQueryable<Course> FilterCourses(this IQueryable<Course> courses,
      FilterOptions options, string? filterValue)
      {
            return options switch

            {
                  FilterOptions.ByPrice => courses.
                  Where(c => c.CourseDetails.CoursePrice <= decimal.Parse(filterValue!)),

                  FilterOptions.ByCategory => courses.Where(c => c.Categories.
                  Any(c => c.CategoryName == filterValue)),

                  _ => courses
            };
      }

      public static IQueryable<Course> PaginatePage(this IQueryable<Course> courses, int page=1)
      {
            int coursesPerPage = 10;

            if (page < 1)
                  throw new ArgumentException("{nameof(page)} can't be less than 0");

            return courses.Skip(page-1 * coursesPerPage).Take(coursesPerPage);

      }


}
