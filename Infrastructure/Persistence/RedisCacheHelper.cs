using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Infrastructure.Persistence
{
    public static class RedisCacheHelper
    {
        public static async Task SetRecordAsync<T>(this IDistributedCache cache,
                                                   string key,
                                                   T value,
                                                   TimeSpan? absoluteExpireTime = null,
                                                   TimeSpan? slidingExpireTime = null)
        {
            var options = new DistributedCacheEntryOptions();

            if (absoluteExpireTime != null)
                options.AbsoluteExpirationRelativeToNow = absoluteExpireTime;
            if (slidingExpireTime != null)
                options.SlidingExpiration = slidingExpireTime;

            var jsonData = JsonConvert.SerializeObject(value);
            await cache.SetStringAsync(key, jsonData, options);
        }

        public static async Task<T?> GetRecordAsync<T>(this IDistributedCache cache,
                                                       string key)
        {
            var jsonData = await cache.GetStringAsync(key);

            if (jsonData is null)
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(jsonData);
        }

        public static async Task<T> DeleteRecordAsync<T>(this IDistributedCache cache,
                                                   string key)
        {
            var jsonData = await cache.GetStringAsync(key);
            await cache.RemoveAsync(key);

            if (jsonData is null)
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(jsonData);
        }
    }
}
