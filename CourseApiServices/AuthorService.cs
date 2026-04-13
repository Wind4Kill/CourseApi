using System;
using CourseApiDomain;
using CourseApiDomain.Entities;
using CourseApiServices.Dtos.AuthorDtos;
using CourseApiServices.Dtos.CourseDtos;
using CourseApiServices.Interfaces.HelpClasses;
using CourseApiServices.Interfaces.Repositories;
using CourseApiServices.Interfaces.Services;

namespace CourseApiServices;

public class AuthorService : IAuthorService
{
      readonly IAuthorRepository _authorRepository;

      public AuthorService(IAuthorRepository authorRepository)
      {
            _authorRepository = authorRepository;
      }

      public async Task<Author> CreateAuthor(CreateAuthorDto authorDto)
      {
            Author author = new Author() { Name = authorDto.AuthorName };
            await _authorRepository.CreateAuthor(author);
            return author;
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
}
