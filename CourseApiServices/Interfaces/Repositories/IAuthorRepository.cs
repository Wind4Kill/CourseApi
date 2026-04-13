using System;
using CourseApiDomain.Entities;

namespace CourseApiServices.Interfaces.Repositories;

public interface IAuthorRepository
{
      Task<List<Author>?> GetAuthorsByNames(List<string> names);
}
