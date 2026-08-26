using System;
using CourseApi.Application.Interfaces.Repositories;
using CourseApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourseApi.Data.Persistency.Repositories;

public class AuthorRepository : IAuthorRepository
{
      readonly ApplicationContext _context;

      public AuthorRepository(ApplicationContext context)
      {
            _context = context;
      }

      public async Task<Author> CreateAuthor(Author author, CancellationToken cancellationToken)
      {
            _context.Authors.Add(author);
            await _context.SaveChangesAsync(cancellationToken);
            return author;
      }

      public async Task DeleteAuthor(Author author, CancellationToken cancellationToken)
      {
            author.IsDeleted = true;
            await _context.SaveChangesAsync(cancellationToken);
      }

      public async Task<Author?> GetAuthorById(int id, CancellationToken cancellationToken)
      {
            Author? requestedAuthor = await _context.Authors.Include(a => a.Courses)!.ThenInclude(c=>c.Reviews).
            SingleOrDefaultAsync(a => a.AuthorId == id, cancellationToken);
            return requestedAuthor;
      }

      public async Task<List<Author>?> GetAuthorsByNames(List<string> names, CancellationToken cancellationToken)
      {
            List<Author>? requestedAuthors = await _context.Authors.
            Where(a=>names.Contains(a.Name)).ToListAsync(cancellationToken);
            return requestedAuthors;
      }

     
}
