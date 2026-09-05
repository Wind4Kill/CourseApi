
using CourseApi.Domain.Entities;

namespace CourseApi.Application.Interfaces.Repositories;

public interface ICategoryRepository
{
      Task<List<Category>?> GetCategoriesByNames(List<string> names, CancellationToken cancellationToken); 
}
