using System;
using System.Text.Json;
using FoodWebApi.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FoodWebApi.Services
{
    public class RedisCacheService : ICacheService
	{
        private readonly IDistributedCache cache;
		public RedisCacheService(IDistributedCache cache)
		{
            this.cache = cache;
		}

        public async Task<T> GetData<T>(string key)
        {
            var jsonData = await cache.GetStringAsync(key);
            if (jsonData is null)
            {
                return default(T);
            }
            return JsonSerializer.Deserialize<T>(jsonData);
        }

        public async Task RemoveData(string key)
        {
            await cache.RemoveAsync(key);
        }

        public async Task SetData<T>(string key, T value, TimeSpan duration)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = duration
            };
            var jsonData = JsonSerializer.Serialize(value);
            await cache.SetStringAsync(key, jsonData, options);
        }
    }
}

