
using System;
using System.Threading.Tasks;
using Application.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Infrastructure.Redis
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDistributedCache _distributedCache;

        public ResponseCacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task CacheResponseAsync(string key, object response, TimeSpan cachingTime)
        {
            if (response == null)
            {
                return;
            }

            await _distributedCache
                .SetStringAsync(
                    key,
                    JsonConvert.SerializeObject(response),
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = cachingTime
                    });
        }

        public async Task<string> GetCachedResponseAsync(string cacheKey)
            => await _distributedCache.GetStringAsync(cacheKey);
    }
}
