
using CourseApi.Domain.Entities;

namespace CourseApi.Application.Interfaces.Repositories;

public interface IAuthorRepository
{
      Task<List<Author>?> GetAuthorsByNames(List<string> names, CancellationToken cancellationToken);

      Author CreateAuthor(Author author);

      Task<Author?> GetAuthorById(int id, CancellationToken cancellationToken);

      void DeleteAuthor(Author author);

}
