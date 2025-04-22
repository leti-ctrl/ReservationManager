using Newtonsoft.Json;
using ReservationManager.Cache.Helper;
using ReservationManager.Cache.Redis;
using ReservationManager.Core.Interfaces.Repositories.Base;
using ReservationManager.DomainModel.Base;
using ReservationManager.Persistence.Repositories.Base;

namespace ReservationManager.Cache.Repositories;

public class CrudBaseTypeCacheRepository<T> : ICrudBaseTypeRepository<T> 
where T : BaseType
{
    private readonly CrudBaseTypeRepository<T> _repository;
    private readonly IRedisService _redisService;

    public CrudBaseTypeCacheRepository(CrudBaseTypeRepository<T> repository, IRedisService redisService)
    {
        _repository = repository;
        _redisService = redisService;
    }

    public async Task<IEnumerable<T>> GetAllTypesAsync(CancellationToken cancellationToken = default)
    {
        var types = await _repository.GetAllTypesAsync(cancellationToken);
        foreach (var type in types)
        {
            var redisKey = BuildKeyHelper.BuildKeyByTypeAndId(typeof(T), type.Id);
            await _redisService.SetIfNotExistsAsync(redisKey, JsonConvert.SerializeObject(type));
        }
        return types;
    }

    

    public async Task<T?> GetTypeById(int id, CancellationToken cancellationToken = default)
    {
        var redisKey = BuildKeyHelper.BuildKeyByTypeAndId(typeof(T), id);
        var redisValue = await _redisService.GetAsync(redisKey);
        if(redisValue != null)
            return JsonConvert.DeserializeObject<T>(redisValue);
        
        var typeValue = await _repository.GetTypeById(id, cancellationToken);
        await _redisService.SetAsync(redisKey, JsonConvert.SerializeObject(typeValue));
        return typeValue;
    }

    public async Task<T?> GetTypeByCode(string code, CancellationToken cancellationToken = default)
    {
        var redisKey = BuildKeyHelper.BuildKeyByTypeAndCode(typeof(T), code);
        var redisValue = await _redisService.GetAsync(redisKey);
        if(redisValue != null)
            return JsonConvert.DeserializeObject<T>(redisValue);

        var typeValue =  await _repository.GetTypeByCode(code, cancellationToken);
        await _redisService.SetAsync(redisKey, JsonConvert.SerializeObject(typeValue));
        return typeValue;
    }

    /// <summary>
    /// Not editable type.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<T> CreateTypeAsync(T entity, CancellationToken cancellationToken = default)
    {
        return await _repository.CreateTypeAsync(entity, cancellationToken);
    }

    /// <summary>
    /// Not editable type.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<T?> UpdateTypeAsync(T entity, CancellationToken cancellationToken = default)
    {
        return await _repository.UpdateTypeAsync(entity, cancellationToken);
    }

    /// <summary>
    /// Not editable type.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    public async Task DeleteTypeAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _repository.DeleteTypeAsync(entity, cancellationToken);
    }
}