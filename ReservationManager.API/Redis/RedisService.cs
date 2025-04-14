using StackExchange.Redis;

namespace ReservationManager.API.Redis;

public class RedisService : IRedisService
{
    private readonly IDatabase _db;

    public RedisService(IConfiguration configuration)
    {
        var redis = ConnectionMultiplexer.Connect(configuration.GetConnectionString("ReservationManagerCache"));
        _db = redis.GetDatabase();
    }

    public async Task SetAsync(string key, string value)
    {
        await _db.StringSetAsync(key, value);
    }

    public async Task<string?> GetAsync(string key)
    {
        return await _db.StringGetAsync(key);
    }
}