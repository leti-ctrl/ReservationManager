using Newtonsoft.Json;
using ReservationManager.Cache.Helper;
using ReservationManager.Cache.Redis;
using ReservationManager.Core.Interfaces.Repositories.Base;
using ReservationManager.DomainModel.Base;
using ReservationManager.Persistence.Repositories.Base;

namespace ReservationManager.Cache.Repositories.Base;

public class CrudEditableTypeCacheRepository<T> : CrudBaseTypeCacheRepository<T>, ICrudEditableTypeRepository<T>
where T : EditableType
{
    private readonly CrudEditableTypeRepository<T> _repository;
    private readonly IRedisService _redisService;
    
    public CrudEditableTypeCacheRepository(CrudEditableTypeRepository<T> repository, IRedisService redisService) 
        : base(repository, redisService)
    {
        _redisService = redisService;
        _repository = repository;
    }

    public override async Task<T> CreateTypeAsync(T typeToCreate, CancellationToken cancellationToken = default)
    {
        var createdType = await _repository.CreateTypeAsync(typeToCreate, cancellationToken);
        
        var redisKey = BuildKeyHelper.BuildKeyByTypeAndId(typeof(T), createdType.Id);
        var serializedData = JsonConvert.SerializeObject(createdType);
        await _redisService.SetAsync(redisKey, serializedData);
        
        return createdType;
    }

    public override async Task<T?> UpdateTypeAsync(T typeToUpdate, CancellationToken cancellationToken = default)
    {
        return await _repository.UpdateTypeAsync(typeToUpdate, cancellationToken);
    }

    public override async Task DeleteTypeAsync(T typeToDelete, CancellationToken cancellationToken = default)
    {
        await _repository.DeleteTypeAsync(typeToDelete, cancellationToken);
        
        var redisKey = BuildKeyHelper.BuildKeyByTypeAndId(typeof(T), typeToDelete.Id);
        await _redisService.RemoveAsync(redisKey);
    }
}