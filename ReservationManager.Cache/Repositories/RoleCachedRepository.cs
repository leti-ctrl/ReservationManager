using ReservationManager.Cache.Redis;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.DomainModel.Meta;
using ReservationManager.Persistence.Repositories;
using ReservationManager.Persistence.Repositories.Base;

namespace ReservationManager.Cache.Repositories;

public class RoleCachedRepository : CrudBaseTypeCacheRepository<Role>, IRoleRepository
{
    public RoleCachedRepository(RoleRepository repository, IRedisService redisService) 
        : base(repository, redisService)
    { }
}