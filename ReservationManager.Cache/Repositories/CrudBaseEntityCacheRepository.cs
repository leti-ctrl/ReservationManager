using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NRedisStack.DataTypes;
using ReservationManager.Cache.Helper;
using ReservationManager.Cache.Redis;
using ReservationManager.Core.Interfaces.Repositories.Base;
using ReservationManager.DomainModel.Base;
using ReservationManager.Persistence.Repositories.Base;

namespace ReservationManager.Cache.Repositories;

public class CrudBaseEntityCacheRepository<T>: ICrudBaseEntityRepository<T>
where T: BaseEntity
{
    private readonly CrudBaseEntityRepository<T> _repository;
    private readonly IRedisService _redisService;

    public CrudBaseEntityCacheRepository(CrudBaseEntityRepository<T> repository, IRedisService redisService)
    {
        _repository = repository;
        _redisService = redisService;
    }
    
    public async Task<IEnumerable<T>> GetAllEntitiesAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.GetAllEntitiesAsync(cancellationToken);
    }

    public async Task<T?> GetEntityByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var redisKey = BuildRedisKeyHelper.BuildKey(typeof(T), id);
        var isValid = await _redisService.IsValid(redisKey);
        if (!isValid)
        {
            var data = await _repository.GetEntityByIdAsync(id, cancellationToken);
            if (data == null)
                return data;
            var newCacheItem = new CacheItem()
            {
                Timestamp = new TimeStamp(DateTime.Now),
                JsonObjectValue = JsonConvert.SerializeObject(data),
            };
            await _redisService.SetAsync(redisKey, newCacheItem);
            return data;
        }

        var cacheItem = await _redisService.GetAsync(redisKey);
        return JsonConvert.DeserializeObject<T>(cacheItem.JsonObjectValue.ToString());
    }

    public async Task<T> CreateEntityAsync(T entity, CancellationToken cancellationToken = default)
    {
        return await _repository.CreateEntityAsync(entity, cancellationToken);
    }

    public async Task<T?> UpdateEntityAsync(T entity, CancellationToken cancellationToken = default)
    {
        return await _repository.UpdateEntityAsync(entity, cancellationToken);
    }

    public async Task DeleteEntityAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _repository.DeleteEntityAsync(entity, cancellationToken);
    }
}