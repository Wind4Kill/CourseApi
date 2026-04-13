using System;
using CourseApiDomain.Entities;

namespace CourseApiServices.Interfaces.Repositories;

public interface ICategoryRepository
{
      Task<List<Category>?> GetCategoriesByNames(List<string> names); 
}
