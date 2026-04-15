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

      public async Task CreateAuthor(Author author)
      {
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
      }

      public async Task<int> DeleteAuthor(int id)
      {
            return await _context.Authors.Where(a => a.AuthorId == id).
            ExecuteUpdateAsync(a => a.
            SetProperty(author => author.IsDeleted, author => true));
      }

      public async Task<Author?> GetAuthorById(int id)
      {
            Author? requestedAuthor = await _context.Authors.Include(a=>a.Courses).SingleOrDefaultAsync(a => a.AuthorId == id);
            return requestedAuthor;
      }

      public async Task<List<Author>?> GetAuthorsByNames(List<string> names)
      {
            List<Author>? requestedAuthors = await _context.Authors.Where(a=>names.Contains(a.Name)).ToListAsync();
            return requestedAuthors;
      }

     
}
