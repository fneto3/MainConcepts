using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PublicApi.Model;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicApi.Infrasctructure.Repositories
{
    public class RedisRepository<T> : IRepository<T> where T : IRedisKey
    {
        private readonly ILogger<RedisRepository<T>> _logger;
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _database;

        public RedisRepository(ILoggerFactory loggerFactory, ConnectionMultiplexer redis)
        {
            _logger = loggerFactory.CreateLogger<RedisRepository<T>>();
            _redis = redis;
            _database = redis.GetDatabase();
        }

        public async Task<bool> DeleteAsync(string key)
        {
            return await _database.KeyDeleteAsync(key);
        }

        public IEnumerable<string> GetKeys()
        {
            var server = GetServer();
            var data = server.Keys();

            return data?.Select(k => k.ToString());
        }

        public async Task<T> GetAsync(string key)
        {
            var data = await _database.StringGetAsync(key);

            if (data.IsNullOrEmpty)
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(data);
        }

        public async Task<T> UpdateAsync(T item)
        {
            var created = await _database.StringSetAsync(item.GetKey(), JsonConvert.SerializeObject(item));

            if (!created)
            {
                _logger.LogInformation("Problem occur persisting the item.");
                return default(T);
            }

            _logger.LogInformation("Basket item persisted succesfully.");

            return await GetAsync(item.GetKey());
        }

        private IServer GetServer()
        {
            var endpoint = _redis.GetEndPoints();
            return _redis.GetServer(endpoint.First());
        }
    }
}
