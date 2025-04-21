namespace ReservationManager.Cache.Redis;

public interface IRedisService
{
    Task SetAsync(string key, string value);
    Task SetIfNotExistsAsync(string itemKey, string serializedItem);
    Task<string?> GetAsync(string key);
    Task RemoveAsync(string key);
}