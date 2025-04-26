using Newtonsoft.Json;
using ReservationManager.Cache.Helper;
using ReservationManager.Cache.Redis;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.DomainModel.Operation;
using ReservationManager.Persistence.Repositories;
using ReservationManager.Persistence.Repositories.Base;

namespace ReservationManager.Cache.Repositories;

public class ResourceCachedRepository : CrudBaseEntityCacheRepository<Resource>, IResourceRepository
{
    private readonly ResourceRepository _repository;
    private readonly IRedisService _redisService;
    
    public ResourceCachedRepository(ResourceRepository repository, IRedisService redisService) 
        : base(repository, redisService)
    {
        _repository = repository;
        _redisService = redisService;
    }

    public async Task<IEnumerable<Resource>> GetFiltered(int? typeId, int? resourceId)
    {
        var resourceList = (await _repository.GetFiltered(typeId, resourceId)).ToList();

        foreach (var resource in resourceList)
        {
            var redisKey = BuildKeyHelper.BuildKeyByTypeAndId(typeof(Resource), resource.Id);
            await _redisService.RefreshOrAddValueAsync(redisKey, JsonConvert.SerializeObject(resource));
        }
        return resourceList;
    }
}