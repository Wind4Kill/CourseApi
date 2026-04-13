using System;
using CourseApiDomain.Entities;
using CourseApiServices.Dtos.AuthorDtos;

namespace CourseApiServices.Interfaces.Repositories;

public interface IAuthorRepository
{
      Task<List<Author>?> GetAuthorsByNames(List<string> names);

      Task CreateAuthor(Author author);

      Task<Author?> GetAuthorById(int id);

}
