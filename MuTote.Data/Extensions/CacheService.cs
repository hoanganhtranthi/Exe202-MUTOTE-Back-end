using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using IDatabase = StackExchange.Redis.IDatabase;

namespace BookStore.Data.Extensions
{
   
    public class CacheService : ICacheService
    {
        //private IDistributedCache _distributedCache;
        private IDatabase redis = ConnectionMultiplexer.Connect("redis-14054.c251.east-us-mz.azure.cloud.redislabs.com:14054,password=f9aeBchTqItgsZfke6Pl1B77ifjEeqXj,abortConnect=false,connectTimeout=30000,responseTimeout=30000").GetDatabase();
    public T GetData<T>(string key)
        {
            /*  var value=_distributedCache.GetString(key);
              if (!string.IsNullOrEmpty(value))
                  return JsonSerializer.Deserialize<T>(value);
              return default;*/
            var value = redis.StringGet(key);
            if (!string.IsNullOrEmpty(value))
                return JsonSerializer.Deserialize<T>(value);
            return default;
        }

        public object RemoveData(string key)
        {
            var _exist = redis.StringGet(key);
            if (!string.IsNullOrEmpty(_exist))
                return redis.KeyDelete(key);
            return false;
        }

        public void SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            var expirty = expirationTime.DateTime.Subtract(DateTime.Now);
            redis.StringSet(key, JsonSerializer.Serialize(value), expirty);
        }
    }
}
