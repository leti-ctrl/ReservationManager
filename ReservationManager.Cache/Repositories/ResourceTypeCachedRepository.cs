using ReservationManager.Cache.Redis;
using ReservationManager.Cache.Repositories.Base;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.DomainModel.Meta;
using ReservationManager.Persistence;
using ReservationManager.Persistence.Repositories;
using ReservationManager.Persistence.Repositories.Base;

namespace ReservationManager.Cache.Repositories;

public class ResourceTypeCachedRepository : CrudEditableTypeCacheRepository<ResourceType>, IResourceTypeRepository
{
    public ResourceTypeCachedRepository(ResourceTypeRepository repository, IRedisService redisService) 
        : base(repository, redisService)
    { }
}