using CourseApi.Application.DTOs;
using CourseApi.Application.DTOs.AuthorDtos;
using CourseApi.Application.DTOs.CategoryDtos;
using CourseApi.Application.DTOs.CourseDtos;
using CourseApi.Application.HelpClasses;
using CourseApi.Application.Interfaces.Repositories;
using CourseApi.Application.Interfaces.Services;
using CourseApi.Domain.Entities;
using CourseApi.Domain.Exceptions;

namespace CourseApi.Application.Authors.Services;

public class AuthorService : IAuthorService
{
      readonly IUnitOfWork _unitOfWork;
      readonly IAuthorRepository _authorRepository;
      readonly ICourseRepository _courseRepository;

      readonly ICategoryRepository _categoryRepository;

      public AuthorService(IUnitOfWork unitOfWork, IAuthorRepository authorRepository, ICourseRepository courseRepository, ICategoryRepository categoryRepository)
      {
            _unitOfWork = unitOfWork;
            _authorRepository = authorRepository;
            _courseRepository = courseRepository;
            _categoryRepository = categoryRepository;
      }

      public async Task<GetAuthorDto> CreateAuthor(CreateAuthorDto authorDto, CancellationToken cancellationToken)
      {
            Author? existedAuthor = (await _authorRepository.
            GetAuthorsByNames(names: [authorDto.AuthorName], cancellationToken))
            ?.FirstOrDefault();

            if (existedAuthor is not null)
            {
                  throw new EntityAlreadyExistsExceptions("Author entity with specified name already exists.");
            }

            Author createdAuthor = new Author() { Name = authorDto.AuthorName };
            createdAuthor = _authorRepository.CreateAuthor(createdAuthor);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            GetAuthorDto mappedAuthor = new GetAuthorDto() { Name = createdAuthor.Name };
            return mappedAuthor;
      }

      public async Task<GetAuthorDto> GetAuthorById(int id, CancellationToken cancellationToken)
      {
            Author? author = await _authorRepository.GetAuthorById(id, cancellationToken);

            if (author is null)
            {
                  throw new EntityNotFoundException("Author hasn't been found");
            }

            GetAuthorDto mappedAuthor = new GetAuthorDto()
            {
                  AuthorId = author.AuthorId,
                  Name = author.Name
            };

            if (author.Courses is not null)
            {
                  mappedAuthor.Courses = author.Courses.Select(c => new GetCourseDto()
                  {
                        CourseId = c.CourseId,
                        CourseName = c.CourseName,
                        CourseRating = c.AverageRating,
                        CoursePrice = c.CourseDetails.CoursePrice
                  }).ToList();
            }

            return mappedAuthor;
      }

      public async Task DeleteAuthor(int id, CancellationToken cancellationToken)
      {
            Author? author = await _authorRepository.GetAuthorById(id, cancellationToken);
            if (author is null)
            {
                  throw new EntityNotFoundException($"Author with {id} ID hasn't been found");
            }
            _authorRepository.DeleteAuthor(author);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
      }

      public async Task<GetCourseByIdDto> AddCourseToAuthor(int authorId, CreateCourseDto courseDto, CancellationToken cancellationToken)
      {

            Course? existingCourse = await _courseRepository.FindCourseByName(courseDto.CourseName, cancellationToken);

            if (existingCourse is not null)
            {
                  throw new EntityAlreadyExistsExceptions($"Entity course with specified name: {existingCourse.CourseName} already exists.");
            }

            Course createdCourse = new Course()
            {
                  CourseName = courseDto.CourseName,
                  CourseDetails = new CourseDetails()
                  {
                        CourseDescription = courseDto.CourseDescription,
                        CoursePrice = courseDto.CoursePrice
                  },
                  Author = new Author { AuthorId = authorId },
            };

            List<Category>? existedCategories = await _categoryRepository.GetCategoriesByNames(names: courseDto.Categories, cancellationToken);

            createdCourse.Categories = await EntityDifferentiator.DifferentiateEntity(dtoNames: courseDto.Categories, existedValues: existedCategories);

            createdCourse = _courseRepository.AddCourse(createdCourse);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            GetCourseByIdDto mappedCourse = new GetCourseByIdDto()
            {
                  CourseId = createdCourse.CourseId,
                  CourseName = createdCourse.CourseName,
                  CourseDescription = createdCourse.CourseDetails.CourseDescription,
                  CoursePrice = createdCourse.CourseDetails.CoursePrice,
                  Author = new GetAuthorDto()
                  {
                        AuthorId = createdCourse.AuthorId,
                        Name = createdCourse.Author.Name
                  },
                  Categories = createdCourse.Categories.Select(c => new GetCategoryDto() { CategoryName = c.Name }).ToList(),
                  CourseRating = createdCourse.AverageRating,
                  Reviews = createdCourse.Reviews is null ? null : createdCourse.Reviews.Select(r => new ReviewDto()
                  {
                        ReviewRating = r.ReviewRating,
                        ReviewText = r.ReviewText
                  }).ToList()
            };

            return mappedCourse;


      }
}
