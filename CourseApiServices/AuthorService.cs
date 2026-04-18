using System;
using CourseApiDomain;
using CourseApiDomain.Entities;
using CourseApiServices.Dtos.AuthorDtos;
using CourseApiServices.Dtos.CourseDtos;
using CourseApiServices.HelpClasses;
using CourseApiServices.HelpClasses.Exceptions;
using CourseApiServices.Interfaces;
using CourseApiServices.Interfaces.HelpClasses;
using CourseApiServices.Interfaces.Repositories;
using CourseApiServices.Interfaces.Services;

namespace CourseApiServices;

public class AuthorService : IAuthorService
{
      readonly IAuthorRepository _authorRepository;
      readonly ICourseRepository _courseRepository;

      readonly ICategoryRepository _categoryRepository;

      public AuthorService(IAuthorRepository authorRepository, ICourseRepository courseRepository, ICategoryRepository categoryRepository)
      {
            _authorRepository = authorRepository;
            _courseRepository = courseRepository;
            _categoryRepository = categoryRepository;
      }

      public async Task<Author> CreateAuthor(CreateAuthorDto authorDto)
      {
            Author? existedAuthor = (await _authorRepository.GetAuthorsByNames(names: [authorDto.AuthorName]))?.FirstOrDefault();

            if (existedAuthor is not null)
            {
                  throw new EntityAlreadyExistsExceptions("Author entity with specified name already exists.");
            }

            Author createdAuthor = new Author() { Name = authorDto.AuthorName };
            await _authorRepository.CreateAuthor(createdAuthor);
            return createdAuthor;
      }

      public async Task<GetAuthorDto> GetAuthorById(int id)
      {
            Author? author = await _authorRepository.GetAuthorById(id);

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
                        CourseRating = CourseFunctions.GetCourseRating(c.CourseId),
                        CoursePrice = c.CourseDetails.CoursePrice
                  }).ToList();
            }

            return mappedAuthor;
      }

      public async Task<int> DeleteAuthor(int id)
      {
            return await _authorRepository.DeleteAuthor(id);
      }

      public async Task<Course> AddCourseToAuthor(int authorId, CreateCourseDto courseDto)
      {

            Course? existingCourse = await _courseRepository.FindCourseByName(courseDto.CourseName);

            if(existingCourse is not null)
            {
                  throw new EntityAlreadyExistsExceptions("Entity course with specified name already exists.");
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

            List<Category>? existedCategories = await _categoryRepository.GetCategoriesByNames(names: courseDto.Categories);

            createdCourse.Categories = await Help.DifferentiateEntity(dtoNames: courseDto.Categories, existedValues: existedCategories);

            await _courseRepository.AddCourse(createdCourse);

            return createdCourse;


      }
}
