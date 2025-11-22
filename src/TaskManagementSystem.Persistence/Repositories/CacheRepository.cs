using Domain.Contracts;
using Microsoft.Extensions.Caching.Memory;

namespace Persistence.Repositories
{
    public class CacheRepository : ICacheRepository
    {

        private readonly IMemoryCache _memoryCache;


        public CacheRepository(IMemoryCache memoryCache)
        {

            this._memoryCache = memoryCache;
        }
        public object? Get(string key)
        {



            return _memoryCache.Get(key);


        }

        public void set(string key, object value, TimeSpan TTL)
        {

            _memoryCache.Set(key, value, TTL);


        }


        public T GetOrAdd<T>(string key, Func<T> acquire, TimeSpan ttl)
        {
            if (_memoryCache.TryGetValue<T>(key, out var cachedValue))
            {
                return cachedValue;
            }

            var value = acquire();
            _memoryCache.Set(key, value, ttl);
            return value;
        }

    }
}
