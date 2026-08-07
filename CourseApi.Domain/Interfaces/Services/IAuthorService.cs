using System;
using CourseApiDomain.Entities;
using CourseApiServices.Dtos.AuthorDtos;
using CourseApiServices.Dtos.CourseDtos;


namespace CourseApiServices.Interfaces.Services;

public interface IAuthorService
{
      Task<Author> CreateAuthor(CreateAuthorDto authorDto, CancellationToken cancellationToken);
      Task<GetAuthorDto> GetAuthorById(int id, CancellationToken cancellationToken);

      Task DeleteAuthor(int id, CancellationToken cancellationToken);

      Task<GetCourseByIdDto> AddCourseToAuthor(int authorId, CreateCourseDto courseDto, CancellationToken cancellationToken);
            

}
