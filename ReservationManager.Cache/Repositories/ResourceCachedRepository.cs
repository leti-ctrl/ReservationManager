using ReservationManager.Cache.Redis;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.DomainModel.Operation;
using ReservationManager.Persistence.Repositories;
using ReservationManager.Persistence.Repositories.Base;

namespace ReservationManager.Cache.Repositories;

public class ResourceCachedRepository : CrudBaseEntityCacheRepository<Resource>, IResourceRepository
{
    private readonly ResourceRepository _repository;
    
    public ResourceCachedRepository(ResourceRepository repository, IRedisService redisService) 
        : base(repository, redisService)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Resource>> GetFiltered(int? typeId, int? resourceId)
    {
        return await _repository.GetFiltered(typeId, resourceId);
    }
}