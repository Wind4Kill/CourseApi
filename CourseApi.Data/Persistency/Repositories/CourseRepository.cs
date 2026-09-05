using CourseApi.Application.Filtration.HelpClasses;
using CourseApi.Application.Interfaces.Repositories;
using CourseApi.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace CourseApi.Data.Persistency.Repositories;



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

      public async Task<List<Course>> GetCourses(SortFilterOptions filterOptions, CancellationToken cancellationToken)
      {
            IQueryable<Course> sortedCourses = SortCourses(_context.Courses.Include(c=>c.Reviews).AsNoTracking(), filterOptions.Sorting);
            IQueryable<Course> filteredCourses = FilterCourses(sortedCourses, filterOptions.Filter, filterOptions.FilterValue);
            IQueryable<Course> paginatedCourses = PaginatePage(filteredCourses, filterOptions.PageNum);
            List<Course> requestedCourses = await paginatedCourses.ToListAsync(cancellationToken);


            return requestedCourses;
      }

      public async Task<Course?> GetCourseById(int id, CancellationToken cancellationToken)
      {
            Course? course = await _context.Courses.
            Include(c => c.Reviews).
            Include(c => c.Author).
            Include(c => c.Categories).
            FirstOrDefaultAsync(c => c.CourseId == id, cancellationToken);

            return course;
      }

      public async Task RemoveCourse(Course course, CancellationToken cancellationToken)
      {
            course.IsDeleted = true;
            await _context.SaveChangesAsync(cancellationToken);
      }

      public async Task UpdateCourse(CancellationToken cancellationToken)
      {
            bool isSaved = false;

            while (!isSaved)
            {
                  try
                  {
                        await _context.SaveChangesAsync(cancellationToken);
                        isSaved = true;
                  }
                  catch (DbUpdateConcurrencyException ex)
                  {
                        foreach (var entry in ex.Entries)
                        {
                              if (entry.Entity is Course)
                              {
                                    var databaseValues = await entry.GetDatabaseValuesAsync();

                                    if (databaseValues is null)
                                    {
                                          throw new InvalidOperationException("Entity has been deleted by another user.");
                                    }

                                    entry.OriginalValues.SetValues(databaseValues);
                              }
                              else
                              {
                                    throw new NotSupportedException("Cocurrency conflict can't be resolved." + entry.Metadata.Name);
                              }
                        }
                  }
            }
      }

      public async Task<Course?> FindCourseByName(string name, CancellationToken cancellationToken)
      {
            Course? requiredCourse = await _context.Courses
            .SingleOrDefaultAsync(c => c.CourseName == name, cancellationToken);

            return requiredCourse;
      }

      private IQueryable<Course> SortCourses(IQueryable<Course> courses,
       SortingOptions options)
      {
            return options switch
            {
                  SortingOptions.ByName => courses.OrderBy(c => c.CourseName),
                  SortingOptions.ByPrice => courses.OrderBy(c => c.CourseDetails.CoursePrice),
                  SortingOptions.Default => courses.OrderBy(c => c.CourseId),
                  _ => courses.OrderBy(c => c.CourseId)
            };
      }

      private IQueryable<Course> FilterCourses(IQueryable<Course> courses,
      FilterOptions options, string? filterValue)
      {
            return options switch

            {
                  FilterOptions.ByPrice => courses.
                  Where(c => c.CourseDetails.CoursePrice <= decimal.Parse(filterValue!)),

                  FilterOptions.ByCategory => courses.Where(c => c.Categories.
                  Any(c => c.Name == filterValue)),

                  FilterOptions.Default => courses,

                  _ => courses
            };
      }

      private IQueryable<Course> PaginatePage(IQueryable<Course> courses, int page = 1)
      {
            int coursesPerPage = 10;

            if (page < 1)
                  throw new ArgumentException("{nameof(page)} can't be less than 0");

            return courses.Skip((page - 1) * coursesPerPage).Take(coursesPerPage);
      }
}
