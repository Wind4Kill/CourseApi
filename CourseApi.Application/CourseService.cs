using CourseApi.Domain.HelpClasses;
using CourseApiDomain;
using CourseApiDomain.Entities;
using CourseApiServices.Dtos.AuthorDtos;
using CourseApiServices.Dtos.CategoryDtos;
using CourseApiServices.Dtos.CourseDtos;
using CourseApiServices.Dtos.ReviewDtos;
using CourseApiServices.Interfaces;
using CourseApiServices.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using CourseApi.Domain.Exceptions;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;


namespace CourseApiServices;

public class CourseService : ICourseService
{
      readonly static SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
      readonly IAuthorRepository _authorRepository;

      readonly ICategoryRepository _categoryRepository;
      readonly ICourseRepository _courseRepository;

      readonly IDistributedCache _cache;

      public CourseService(ICourseRepository courseRepository, IAuthorRepository authorRepository,
      ICategoryRepository categoryRepository, IDistributedCache cache)
      {
            _courseRepository = courseRepository;
            _authorRepository = authorRepository;
            _categoryRepository = categoryRepository;
            _cache = cache;
      }

      public async Task<GetCourseByIdDto> CreateCourse(CreateCourseDto dto, CancellationToken cancellationToken)
      {

            Course addedCourse = new Course()
            {
                  CourseName = dto.CourseName,
                  CourseDetails = new CourseDetails()
                  {
                        CourseDescription = dto.CourseDescription,
                        CoursePrice = dto.CoursePrice
                  },
            };
            Author? existedAuthor = (await _authorRepository.GetAuthorsByNames([dto.Author], cancellationToken))?.FirstOrDefault();

            if (existedAuthor is not null)
            {
                  addedCourse.Author = existedAuthor;
            }
            else
            {
                  addedCourse.Author = new Author() { Name = dto.Author };
            }

            List<Category>? existedCategories = await _categoryRepository.GetCategoriesByNames(dto.Categories, cancellationToken);
            addedCourse.Categories = await EntityDifferentiator.DifferentiateEntity<Category>(dtoNames: dto.Categories, existedValues: existedCategories);

            addedCourse = await _courseRepository.AddCourse(addedCourse, cancellationToken);

            GetCourseByIdDto mappedCourse = new GetCourseByIdDto()
            {
                  CourseId = addedCourse.CourseId,
                  CourseName = addedCourse.CourseName,
                  CourseDescription = addedCourse.CourseDetails.CourseDescription,
                  CoursePrice = addedCourse.CourseDetails.CoursePrice,
                  CourseRating = addedCourse.AverageRating,
                  Author = new GetAuthorDto() { AuthorId = addedCourse.AuthorId, Name = addedCourse.Author.Name },
                  Categories = addedCourse.Categories.Select(c => new GetCategoryDto() { CategoryName = c.Name }).ToList(),
                  Reviews = addedCourse.Reviews is null ? null :
                  addedCourse.Reviews.Select(r => new ReviewDto()
                  {
                        ReviewRating = r.ReviewRating,
                        ReviewText = r.ReviewText
                  }).ToList()
            };

            return mappedCourse;
      }

      public async Task<List<GetCourseDto>> GetCourses(SortFilterOptions options, CancellationToken cancellationToken)
      {
            IQueryable<Course> courses = _courseRepository.GetCourses();

            List<GetCourseDto> mappedCourses = await courses.
                        SortCourses(options.Sorting).
                        FilterCourses(options.Filter, options.FilterValue).
                        PaginatePage(options.PageNum).
                        Select(c => new GetCourseDto
                        {
                              CourseId = c.CourseId,
                              CourseName = c.CourseName,
                              CoursePrice = c.CourseDetails.CoursePrice,
                              CourseRating = CourseFunctions.GetCourseRating(c.CourseId)
                        }).ToListAsync(cancellationToken);

            return mappedCourses;
      }

      public async Task<GetCourseByIdDto?> GetCourseById(int id, CancellationToken cancellationToken)
      {

            string key = GetKeyString(id);

            string? cachedCourse = await _cache.GetStringAsync(key, cancellationToken);

            if (cachedCourse is not null)
            {
                  GetCourseByIdDto requestedCourse = JsonSerializer.Deserialize<GetCourseByIdDto>(cachedCourse)!;

                  return requestedCourse;

            }
            
                  await _semaphore.WaitAsync(cancellationToken);

                  try
                  {
                        cachedCourse = await _cache.GetStringAsync(key, cancellationToken);
                        if (cachedCourse is not null)
                        {
                              GetCourseByIdDto requestedCourse = JsonSerializer.Deserialize<GetCourseByIdDto>(cachedCourse)!;
                              return requestedCourse;
                        }

                        Course requested = await SearchForCourse(id, cancellationToken);


                        GetCourseByIdDto mappedCourse = new GetCourseByIdDto()
                        {
                              CourseId = requested.CourseId,
                              CourseName = requested.CourseName,
                              CoursePrice = requested.CourseDetails.CoursePrice,
                              CourseDescription = requested.CourseDetails.CourseDescription,
                              CourseRating = requested.AverageRating,
                              Author = new GetAuthorDto()
                              {
                                    AuthorId = requested.Author.AuthorId,
                                    Name = requested.Author.Name
                              },
                              Categories = requested.Categories.
                        Select(c => new GetCategoryDto
                        {
                              CategoryName = c.Name
                        }).
                        ToList(),
                              Reviews = requested.Reviews is null ? null :
                              requested.Reviews.Select(r => new ReviewDto()
                              {
                                    ReviewText = r.ReviewText,
                                    ReviewRating = r.ReviewRating
                              }).ToList()
                        };

                        DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                        {
                              AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2),
                              SlidingExpiration = TimeSpan.FromHours(1)
                        };
                        await _cache.SetStringAsync(key, JsonSerializer.Serialize(mappedCourse), options, cancellationToken);

                        return mappedCourse;
                  }
                  finally
                  {
                        _semaphore.Release();
                  }
      }

      public async Task RemoveCourse(int id, CancellationToken cancellationToken)
      {
            string key = GetKeyString(id);
            Course requestedCourse = await SearchForCourse(id, cancellationToken);
            await _courseRepository.RemoveCourse(requestedCourse, cancellationToken);
            _cache.Remove(key);
      }

      public async Task UpdateCourse(int id, UpdateCourseDto updateCourseDto, CancellationToken cancellationToken)
      {
            string key = GetKeyString(id);
            Course requiredCourse = await SearchForCourse(id, cancellationToken);

            if (!string.IsNullOrEmpty(updateCourseDto.CourseName) && !updateCourseDto.CourseName.Equals(requiredCourse.CourseName))
            {
                  requiredCourse.CourseName = updateCourseDto.CourseName;
            }

            if (!string.IsNullOrEmpty(updateCourseDto.CourseDescription) && !updateCourseDto.CourseDescription.Equals(requiredCourse.CourseDetails.CourseDescription))
            {
                  requiredCourse.CourseDetails.CourseDescription = updateCourseDto.CourseDescription;
            }

            if (updateCourseDto.CoursePrice.HasValue && !updateCourseDto.CoursePrice.Equals(requiredCourse.CourseDetails.CoursePrice) && updateCourseDto.CoursePrice is not 0)
            {
                  requiredCourse.CourseDetails.CoursePrice = updateCourseDto.CoursePrice.Value;
            }

            if (!string.IsNullOrEmpty(updateCourseDto.Author) && requiredCourse.Author.Name != updateCourseDto.Author)
            {
                  Author? existedAuthor = (await _authorRepository.GetAuthorsByNames([updateCourseDto.Author], cancellationToken))?.FirstOrDefault();

                  if (existedAuthor is not null)
                  {
                        requiredCourse.Author = existedAuthor;
                  }
                  else
                  {
                        requiredCourse.Author = new Author() { Name = updateCourseDto.Author };
                  }
            }

            if (updateCourseDto.Categories is not null && updateCourseDto.Categories.Any(c => c is not null))
            {
                  var existedCategories = await _categoryRepository.GetCategoriesByNames(updateCourseDto.Categories, cancellationToken);
                  requiredCourse.Categories = await EntityDifferentiator.DifferentiateEntity<Category>(updateCourseDto.Categories, existedCategories);
            }

            bool isSaved = false;

            while (!isSaved)
            {
                  try
                  {
                        await _courseRepository.UpdateCourse(cancellationToken);
                        isSaved = true;
                        _cache.Remove(key);
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

      private async Task<Course> SearchForCourse(int id, CancellationToken cancellationToken)
      {
            Course? requestedCourse = await _courseRepository.GetCourseById(id, cancellationToken);

            if (requestedCourse is null)
            {
                  throw new EntityNotFoundException($"Course with {id} ID hasn't been found");
            }

            return requestedCourse!;
      }

      private string GetKeyString(int id)
      {
            return $"Course:{id}";
      }

}
