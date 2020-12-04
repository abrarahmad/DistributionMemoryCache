using System;
using System.Threading.Tasks;

namespace SampleDemoDMC.Services
{
    public interface IDopDistributionCache
    {
        public Task<T> GetOrCreate<T>(string key, Func<Task<T>> func) where T : class;
        public Task<T> Get<T>(string key) where T : class;
        public Task Set<T>(string key, T value) where T : class;
        public Task Remove(string key);
    }
}
