using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseApi.Application.Interfaces.Services
{
    public interface ICacheService<T> where T : class
    {
        Task AddToCacheAsync(T entity, int id, CancellationToken token);

        Task<T?> TryGetValueAsync(Type entityType, int id, CancellationToken token);

        Task RemoveFromCacheAsync(Type entityType, int id, CancellationToken token);


    }
}