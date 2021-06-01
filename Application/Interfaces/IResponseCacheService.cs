

using System;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IResponseCacheService
    {
        Task CacheResponseAsync(string key, object response, TimeSpan cachingTime);

        Task<string> GetCachedResponseAsync(string cacheKey);
    }
}
