using System;
using CourseApiDomain;
using CourseApiDomain.Entities;
using CourseApiServices.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CourseApiServices;

public class CategoryRepository:ICategoryRepository
{
      readonly ApplicationContext _context;

      public CategoryRepository(ApplicationContext context)
      {
            _context = context;
      }

      public async Task<List<Category>?> GetCategoriesByNames(List<string> names)
      {
            List<Category>? requestedCategories = await _context.Categories.Where(c=>names.Contains(c.Name)).ToListAsync();

            return requestedCategories;
      }

}
