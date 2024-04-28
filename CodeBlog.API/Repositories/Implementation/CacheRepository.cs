using CodeBlog.API.Repositories.Interface;
using StackExchange.Redis;
using System.Data;
using System.Text.Json;

namespace CodeBlog.API.Repositories.Implementation
{
    public class CacheRepository : ICacheRepository
    {
        private readonly IConfiguration _conf;
        private IDatabase _cacheDb;
        public CacheRepository(IConfiguration conf)
        {
            _conf = conf;
            var redisConnectionStr = _conf.GetConnectionString("Redis");
            var redis = ConnectionMultiplexer.Connect(redisConnectionStr);
            _cacheDb = redis.GetDatabase();
        }

        public T GetData<T>(string key)
        {
            var data = _cacheDb.StringGet(key);

            if (!string.IsNullOrEmpty(data))
                return JsonSerializer.Deserialize<T>(data);

            return default;
        }

        public object RemoveData<T>(string key)
        {
            var exist = _cacheDb.KeyExists(key);

            if (exist)
                return _cacheDb.KeyDelete(key);

            return false;
        }

        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            var expireTime = expirationTime.DateTime.Subtract(DateTime.Now);
            return _cacheDb.StringSet(key, JsonSerializer.Serialize(value), expireTime);
        }
    }
}
