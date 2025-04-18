
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace SurveyBasket.Api.Services
{
    public class CacheService(IDistributedCache distributedCache) : ICacheService
    {
        private readonly IDistributedCache _distributedCache = distributedCache;

        public async Task<T?> GetAsync<T>(string key) where T : class
        {
            var result = await _distributedCache.GetStringAsync(key);
            return string.IsNullOrEmpty(result)
                ? null 
                : JsonSerializer.Deserialize<T>(result);
        }
        public async Task SetAsync<T>(string key, T value) where T : class
        {
            await _distributedCache.SetStringAsync(key, JsonSerializer.Serialize(value));
        }

        public async Task RemoveAsync(string key) 
        {
            await _distributedCache.RefreshAsync(key);
        }

    }
}
