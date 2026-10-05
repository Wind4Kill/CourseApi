using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CourseApi.Application.Interfaces.Services;
using Microsoft.Extensions.Caching.Distributed;

namespace CourseApi.Data.Caching
{
    public class CacheService<T>(IDistributedCache cache) : ICacheService<T> where T : class
    {
        public async Task AddToCacheAsync(T entity, int id, CancellationToken token)
        {
            string key = GetCacheKey(entity.GetType(), id);
            string serializedEntity = JsonSerializer.Serialize<T>(entity);

            DistributedCacheEntryOptions options = new()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60),
                SlidingExpiration = TimeSpan.FromMinutes(15),
            };

            await cache.SetStringAsync(key, serializedEntity, options, token);
        }

        public async Task<T?> TryGetValueAsync(T entity, int id, CancellationToken token)
        {
            string key = GetCacheKey(typeof(T), id);
            string? cachedSerializedValue = await cache.GetStringAsync(key, token);
            if (cachedSerializedValue is null)
            {
                return default;
            }
            T cachedEntity = JsonSerializer.Deserialize<T>(cachedSerializedValue)!;
            return cachedEntity;
        }

        public async Task RemoveFromCacheAsync(T entity, int id, CancellationToken token)
        {
            string key = GetCacheKey(typeof(T), id);
            await cache.RemoveAsync(key, token);
        }

        private string GetCacheKey(Type type, int id) => $"{type.Name}:{id}";
    }
}