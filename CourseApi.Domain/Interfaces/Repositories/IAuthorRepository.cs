using System;
using CourseApiDomain.Entities;
using CourseApiServices.Dtos.AuthorDtos;

namespace CourseApiServices.Interfaces.Repositories;

public interface IAuthorRepository
{
      Task<List<Author>?> GetAuthorsByNames(List<string> names, CancellationToken cancellationToken);

      Task<Author> CreateAuthor(Author author, CancellationToken cancellationToken);

      Task<Author?> GetAuthorById(int id, CancellationToken cancellationToken);

      Task DeleteAuthor(int id, CancellationToken cancellationToken);

}
