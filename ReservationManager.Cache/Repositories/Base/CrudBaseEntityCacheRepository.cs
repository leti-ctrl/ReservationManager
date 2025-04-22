using Newtonsoft.Json;
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
        var entities = await _repository.GetAllEntitiesAsync(cancellationToken);
        foreach (var entity in entities)
        {
            var redisKey = BuildKeyHelper.BuildKeyByTypeAndId(typeof(T), entity.Id);
            await _redisService.SetIfNotExistsAsync(redisKey, JsonConvert.SerializeObject(entity));
        }
        return entities;
    }

    public async Task<T?> GetEntityByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var redisKey = BuildKeyHelper.BuildKeyByTypeAndId(typeof(T), id);
        var redisValue = await _redisService.GetAsync(redisKey);
        if(redisValue != null)
            return JsonConvert.DeserializeObject<T>(redisValue);
       
        var repositoryData = await _repository.GetEntityByIdAsync(id, cancellationToken);
        if (repositoryData == null)
            return null;
        
        var serializedData = JsonConvert.SerializeObject(repositoryData);
        await _redisService.SetAsync(redisKey, serializedData);
        return repositoryData;
    }

    public async Task<T> CreateEntityAsync(T entity, CancellationToken cancellationToken = default)
    {
        var newEntity = await _repository.CreateEntityAsync(entity, cancellationToken);

        var entityRedisKey = BuildKeyHelper.BuildKeyByTypeAndId(typeof(T), newEntity.Id);
        var serializedData = JsonConvert.SerializeObject(newEntity);
        await _redisService.SetAsync(entityRedisKey, serializedData);
        
        return newEntity;
    }

    public async Task<T?> UpdateEntityAsync(T entity, CancellationToken cancellationToken = default)
    {
        return await _repository.UpdateEntityAsync(entity, cancellationToken);
    }

    public async Task DeleteEntityAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _repository.DeleteEntityAsync(entity, cancellationToken);
        
        var deletedEntity =  BuildKeyHelper.BuildKeyByTypeAndId(typeof(T), entity.Id);
        await _redisService.RemoveAsync(deletedEntity);
    }
}