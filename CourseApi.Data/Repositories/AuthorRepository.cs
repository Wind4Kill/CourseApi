using System;
using CourseApiDomain;
using CourseApiDomain.Entities;
using CourseApiServices.Dtos.AuthorDtos;
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

      public async Task<Author> CreateAuthor(Author author, CancellationToken cancellationToken)
      {
            _context.Authors.Add(author);
            await _context.SaveChangesAsync(cancellationToken);
            return author;
      }

      public async Task DeleteAuthor(int id, CancellationToken cancellationToken)
      {
            await _context.Authors.Where(a => a.AuthorId == id).
            ExecuteUpdateAsync(a => a.
            SetProperty(author => author.IsDeleted, author => true), cancellationToken);
      }

      public async Task<Author?> GetAuthorById(int id, CancellationToken cancellationToken)
      {
            Author? requestedAuthor = await _context.Authors.Include(a => a.Courses).
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
