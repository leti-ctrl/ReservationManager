﻿using Newtonsoft.Json;
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
        var types = (await _repository.GetAllTypesAsync(cancellationToken)).ToList();
        foreach (var type in types)
        {
            var redisKey = BuildKeyHelper.BuildKeyByTypeAndId(typeof(T), type.Id);
            await _redisService.RefreshOrAddValueAsync(redisKey, JsonConvert.SerializeObject(type));
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
        await _redisService.RefreshOrAddValueAsync(redisKey, JsonConvert.SerializeObject(typeValue));
        return typeValue;
    }

    public async Task<T?> GetTypeByCode(string code, CancellationToken cancellationToken = default)
    {
        var redisKey = BuildKeyHelper.BuildKeyByTypeAndCode(typeof(T), code);
        var redisValue = await _redisService.GetAsync(redisKey);
        if(redisValue != null)
            return JsonConvert.DeserializeObject<T>(redisValue);

        var typeValue =  await _repository.GetTypeByCode(code, cancellationToken);
        await _redisService.RefreshOrAddValueAsync(redisKey, JsonConvert.SerializeObject(typeValue));
        return typeValue;
    }

    /// <summary>
    /// Not cached because is not editable type.
    /// </summary>
    /// <remarks>Not cached</remarks>
    /// <returns>throw notImplementedException</returns>
    public virtual async Task<T> CreateTypeAsync(T typeToCreate, CancellationToken cancellationToken = default)
    {
        return await _repository.CreateTypeAsync(typeToCreate, cancellationToken);
    }

    /// <summary>
    /// Not cached because is not editable type.
    /// </summary>
    /// <remarks>Not cached</remarks>
    /// <returns>throw notImplementedException</returns>
    public virtual async Task<T?> UpdateTypeAsync(T typeToUpdate, CancellationToken cancellationToken = default)
    {
        return await _repository.UpdateTypeAsync(typeToUpdate, cancellationToken);
    }

    /// <summary>
    /// Not cached because is not editable type.
    /// </summary>
    /// <remarks>Not cached</remarks>
    /// <returns>throw notImplementedException</returns>
    public virtual async Task DeleteTypeAsync(T typeToDelete, CancellationToken cancellationToken = default)
    {
        await _repository.DeleteTypeAsync(typeToDelete, cancellationToken);
    }
}