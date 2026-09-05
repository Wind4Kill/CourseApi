
using CourseApi.Application.Interfaces.Repositories;
using CourseApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourseApi.Data.Persistency.Repositories;

public class CategoryRepository:ICategoryRepository
{
      readonly ApplicationContext _context;

      public CategoryRepository(ApplicationContext context)
      {
            _context = context;
      }

      public async Task<List<Category>?> GetCategoriesByNames(List<string> names, CancellationToken cancellationToken)
      {
            List<Category>? requestedCategories = await _context.Categories.
            Where(c=>names.Contains(c.Name)).ToListAsync(cancellationToken);

            return requestedCategories;
      }

}
