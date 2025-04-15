using System.Runtime.InteropServices.JavaScript;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NRedisStack.DataTypes;
using NRedisStack.RedisStackCommands;
using StackExchange.Redis;

namespace ReservationManager.Cache.Redis;

public class RedisService : IRedisService
{
    private readonly IDatabase _db;

    public RedisService(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ReservationManagerCache");
        var redis = ConnectionMultiplexer.Connect(connectionString!);
        _db = redis.GetDatabase();
    }

    public async Task SetAsync(string key, CacheItem cacheItem)
    {
        var value = JsonConvert.SerializeObject(cacheItem);
        await _db.StringSetAsync(key, value, TimeSpan.FromSeconds(10));
    }

    public async Task<CacheItem?> GetAsync(string key)
    {
        var item = await _db.StringGetAsync(key);
        return !item.HasValue ? null : JsonConvert.DeserializeObject<CacheItem>(item!);
    }

    public async Task<bool> IsValid(string key)
    {
        var item = await _db.StringGetAsync(key);
        if (!item.HasValue)
            return false;
        var cacheItem = JsonConvert.DeserializeObject<CacheItem>(item!);
        var currentTimestamp = new TimeStamp(DateTime.Now.AddMinutes(5));
        return cacheItem!.Timestamp < currentTimestamp;
    }
}