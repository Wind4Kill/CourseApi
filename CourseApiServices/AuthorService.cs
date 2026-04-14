using System;
using CourseApiDomain;
using CourseApiDomain.Entities;
using CourseApiServices.Dtos.AuthorDtos;
using CourseApiServices.Dtos.CourseDtos;
using CourseApiServices.Interfaces;
using CourseApiServices.Interfaces.HelpClasses;
using CourseApiServices.Interfaces.Repositories;
using CourseApiServices.Interfaces.Services;

namespace CourseApiServices;

public class AuthorService : IAuthorService
{
      readonly IAuthorRepository _authorRepository;
      readonly ICourseRepository _courseRepository;

      public AuthorService(IAuthorRepository authorRepository, ICourseRepository courseRepository)
      {
            _authorRepository = authorRepository;
            _courseRepository = courseRepository;
      }

      public async Task<Author> CreateAuthor(CreateAuthorDto authorDto)
      {
            Author createdAuthor = new Author() { Name = authorDto.AuthorName };
            await _authorRepository.CreateAuthor(createdAuthor);
            return createdAuthor;
      }

      public async Task<GetAuthorByIdDto> GetAuthorById(int id)
      {
            Author? author = await _authorRepository.GetAuthorById(id);

            if (author is null)
            {
                  throw new EntityNotFoundException("Author hasn't been found");
            }

            GetAuthorByIdDto mappedAuthor = new GetAuthorByIdDto()
            {
                  AuthorId = author.AuthorId,
                  AuthorName = author.Name,
            };

            if (author.Courses is not null)
            {
                  mappedAuthor.Courses = author.Courses.Select(c => new GetCourseByIdDto()
                  {
                        CourseId = c.CourseId,
                        CourseName = c.CourseName,
                        CourseRating = CourseFunctions.GetCourseRating(c.CourseId),
                        CoursePrice = c.CourseDetails.CoursePrice,
                        CourseDescription = c.CourseDetails.CourseDescription
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

            Course createdCourse = new Course()
            {
                  CourseName = courseDto.CourseName,
                  CourseDetails = new CourseDetails()
                  {
                        CourseDescription = courseDto.CourseDescription,
                        CoursePrice = courseDto.CoursePrice
                  },
                  Authors = new List<Author>() { new Author { AuthorId = authorId } },
                  Categories=new List<Category>()            };

            foreach (var category in courseDto.Categories)
            {
                  createdCourse.Categories.Add(new Category { Name = category.CategoryName });
            }

            await _courseRepository.AddCourse(createdCourse);

            return createdCourse;
            

      }
}
