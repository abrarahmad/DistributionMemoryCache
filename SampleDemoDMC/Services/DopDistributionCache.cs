using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SampleDemoDMC.Model;

namespace SampleDemoDMC.Services
{
    public class DopDistributionCache : IDopDistributionCache
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IOptions<CacheConfiguration> _cacheConfiguration;
        private readonly ILogger<DopDistributionCache> _logger;
        public DopDistributionCache(ILogger<DopDistributionCache> logger, IDistributedCache distributedCache, IOptions<CacheConfiguration> cacheConfiguration) =>
           (_logger, _distributedCache, _cacheConfiguration) = (logger, distributedCache, cacheConfiguration);

        public async Task<T> Get<T>(string key) where T : class
        {
            var value = await _distributedCache.GetStringAsync(key).ConfigureAwait(false);
            if (value is null)
                return default;
            return JsonConvert.DeserializeObject<T>(value);
        }

        public async Task<T> GetOrCreate<T>(string key, Func<Task<T>> func) where T : class
        {
            var value = await _distributedCache.GetStringAsync(key).ConfigureAwait(false);
            if (value is null)
            {
                //calling source function to get data
                var result = await func().ConfigureAwait(false);

                await Set(key, result).ConfigureAwait(false);
                _logger.LogInformation("Key = {Key}, data returing from repository", key);
                return result;
            }
            _logger.LogInformation("Key = {Key}, data returing from cache", key);
            return JsonConvert.DeserializeObject<T>(value);
        }

        public async Task Set<T>(string key, T value) where T : class
        {
            DistributedCacheEntryOptions distributedCacheEntryOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(_cacheConfiguration.Value.CacheTimeout),
                SlidingExpiration = TimeSpan.FromSeconds(_cacheConfiguration.Value.CacheTimeout)
            };
            await _distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(value), distributedCacheEntryOptions)
                .ConfigureAwait(false);
        }
        public async Task Remove(string key)
        {
            await _distributedCache.RemoveAsync(key)
                .ConfigureAwait(false);
        }
    }
}
