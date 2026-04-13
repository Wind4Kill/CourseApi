using System;
using CourseApiDomain;
using CourseApiDomain.Entities;
using CourseApiServices.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CourseApiServices.Interfaces.Services;

public class AuthorRepository : IAuthorRepository
{
      readonly ApplicationContext _context;

      public AuthorRepository(ApplicationContext context)
      {
            _context = context;
      }

      public async Task<List<Author>?> GetAuthorsByNames(List<string> names)
      {
            List<Author>? requestedAuthors = await _context.Authors.Where(a=>names.Contains(a.Name)).ToListAsync();
            return requestedAuthors;
      }
}
