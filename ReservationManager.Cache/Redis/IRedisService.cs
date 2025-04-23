namespace ReservationManager.Cache.Redis;

public interface IRedisService
{
    Task RefreshOrAddValueAsync(string itemKey, string serializedItem);
    Task<string?> GetAsync(string key);
    Task RemoveAsync(string key);
}