using CourseApi.Domain.Exceptions;
using System.Text.Json;
using CourseApi.Application.Interfaces.Services;
using CourseApi.Application.DTOs.CourseDtos;
using CourseApi.Domain.Entities;
using CourseApi.Application.HelpClasses;
using CourseApi.Application.DTOs;
using CourseApi.Application.DTOs.CategoryDtos;
using CourseApi.Application.DTOs.AuthorDtos;
using CourseApi.Application.Filtration.HelpClasses;
using Microsoft.Extensions.Caching.Distributed;
using CourseApi.Application.Interfaces.Repositories;
using CourseApi.Application.Reviews;


namespace CourseApi.Application.Services;

public class CourseService : ICourseService
{
      readonly IUnitOfWork _unitOfWork;
      readonly static SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
      readonly IAuthorRepository _authorRepository;
      readonly ICategoryRepository _categoryRepository;
      readonly ICourseRepository _courseRepository;

      readonly IReviewRepository _reviewRepository;
      readonly ICacheService<Course> _cache;

      public CourseService(IUnitOfWork unitOfWork, ICourseRepository courseRepository, IAuthorRepository authorRepository,
      ICategoryRepository categoryRepository, IReviewRepository reviewRepository, ICacheService<Course> cache)
      {
            _unitOfWork = unitOfWork;
            _courseRepository = courseRepository;
            _authorRepository = authorRepository;
            _categoryRepository = categoryRepository;
            _reviewRepository = reviewRepository;
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

            addedCourse = _courseRepository.AddCourse(addedCourse);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

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
            List<Course> courses = await _courseRepository.GetCourses(options, cancellationToken);

            List<GetCourseDto> mappedCourses = courses.
                        Select(c => new GetCourseDto
                        {
                              CourseId = c.CourseId,
                              CourseName = c.CourseName,
                              CoursePrice = c.CourseDetails.CoursePrice,
                              CourseRating = c.AverageRating
                        }).ToList();

            return mappedCourses;
      }

      public async Task<GetCourseByIdDto?> GetCourseById(int id, CancellationToken cancellationToken)
      {
            Course? requestedCourse = await _cache.TryGetValueAsync(typeof(Course), id, cancellationToken);
            GetCourseByIdDto mappedCourse;
            if (requestedCourse is null)
            {
                  await _semaphore.WaitAsync(cancellationToken);
                  try
                  {

                        requestedCourse = await SearchForCourse(id, cancellationToken);
                        await _cache.AddToCacheAsync(requestedCourse, requestedCourse.CourseId, cancellationToken);
                  }
                  finally
                  {
                        _semaphore.Release();
                  }
            }

            mappedCourse = new GetCourseByIdDto()
            {
                  CourseId = requestedCourse.CourseId,
                  CourseName = requestedCourse.CourseName,
                  CoursePrice = requestedCourse.CourseDetails.CoursePrice,
                  CourseDescription = requestedCourse.CourseDetails.CourseDescription,
                  CourseRating = requestedCourse.AverageRating,
                  Author = new GetAuthorDto()
                  {
                        AuthorId = requestedCourse.Author.AuthorId,
                        Name = requestedCourse.Author.Name
                  },
                  Categories = requestedCourse.Categories.
            Select(c => new GetCategoryDto
            {
                  CategoryName = c.Name
            }).
            ToList(),
                  Reviews = requestedCourse.Reviews is null ? null :
                  requestedCourse.Reviews.Select(r => new ReviewDto()
                  {
                        ReviewText = r.ReviewText,
                        ReviewRating = r.ReviewRating
                  }).ToList()
            };

            return mappedCourse;

      }

      public async Task RemoveCourse(int id, CancellationToken cancellationToken)
      {
            Course requestedCourse = await SearchForCourse(id, cancellationToken);
            _courseRepository.RemoveCourse(requestedCourse);
            await _unitOfWork.SaveChangesAsync();
            await _cache.RemoveFromCacheAsync(typeof(Course), id, cancellationToken);
      }

      public async Task UpdateCourse(int id, UpdateCourseDto updateCourseDto, CancellationToken cancellationToken)
      {
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


            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _cache.RemoveFromCacheAsync(typeof(Course), id, cancellationToken);
      }

      public async Task<GetReviewDto> AddReviewToCourse(int courseId, ReviewDto reviewDto, CancellationToken cancellationToken)
      {
            Course requestedCourse = await SearchForCourse(courseId, cancellationToken);

            Review addedReview = new Review()
            {
                  CourseId = requestedCourse.CourseId,
                  ReviewText = reviewDto.ReviewText,
                  ReviewRating = reviewDto.ReviewRating
            };

            _reviewRepository.AddReview(addedReview);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            GetReviewDto mappedReview = new GetReviewDto()
            {
                  ReviewId = addedReview.CourseId,
                  ReviewText = addedReview.ReviewText!,
                  ReviewRating = addedReview.ReviewRating
            };
            return mappedReview;
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
}
