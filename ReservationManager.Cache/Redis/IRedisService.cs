namespace ReservationManager.Cache.Redis;

public interface IRedisService
{
    Task SetAsync(string key, CacheItem cacheItem);
    Task<CacheItem?> GetAsync(string key);
    Task<bool> IsValid(string key);
}