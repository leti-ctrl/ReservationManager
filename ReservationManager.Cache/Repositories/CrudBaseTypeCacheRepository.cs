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
        return await _repository.GetAllTypesAsync(cancellationToken);
    }

    public async Task<T?> GetTypeById(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.GetTypeById(id, cancellationToken);
    }

    public async Task<T?> GetTypeByCode(string code, CancellationToken cancellationToken = default)
    {
        return await _repository.GetTypeByCode(code, cancellationToken);
    }

    public async Task<T> CreateTypeAsync(T entity, CancellationToken cancellationToken = default)
    {
        return await _repository.CreateTypeAsync(entity, cancellationToken);
    }

    public async Task<T?> UpdateTypeAsync(T entity, CancellationToken cancellationToken = default)
    {
        return await _repository.UpdateTypeAsync(entity, cancellationToken);
    }

    public async Task DeleteTypeAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _repository.DeleteTypeAsync(entity, cancellationToken);
    }
}