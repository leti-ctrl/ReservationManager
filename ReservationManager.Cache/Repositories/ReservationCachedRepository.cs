using Newtonsoft.Json;
using ReservationManager.Cache.Helper;
using ReservationManager.Cache.Redis;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.DomainModel.Operation;
using ReservationManager.Persistence;
using ReservationManager.Persistence.Repositories;

namespace ReservationManager.Cache.Repositories;

public class ReservationCachedRepository : CrudBaseEntityCacheRepository<Reservation>, IReservationRepository
{
    private readonly ReservationRepository _repository;
    private readonly IRedisService _redisService;

    public ReservationCachedRepository(ReservationManagerDbContext context, ReservationRepository repository, IRedisService redisService) 
        : base(repository, redisService)
    {
        _repository = repository;
        _redisService = redisService;
    }
    
    public async Task<IEnumerable<Reservation>> GetReservationByUserIdFromToday(int userId)
    {
        return await GetOrSetCacheAsync(
            BuildKeyHelper.BuildKeyByTypeIdAndValue(typeof(User), userId, typeof(Reservation)),
            () => _repository.GetReservationByUserIdFromToday(userId)
        );
    }

    /// <summary>
    /// Userd by ResourceFilterService for retrieve filtered resources.
    /// Not cached because need always data from db. 
    /// </summary>
    /// <param name="resourceIds"></param>
    /// <param name="startDate"></param>
    /// <param name="startTime"></param>
    /// <param name="endTime"></param>
    /// <returns>List of reservation</returns>
    public async Task<IEnumerable<Reservation>> GetReservationByResourceDateTimeAsync(List<int> resourceIds, DateOnly startDate, TimeOnly startTime, TimeOnly endTime)
    {
        return await _repository.GetReservationByResourceDateTimeAsync(resourceIds, startDate, startTime, endTime);
    }

    public async Task<IEnumerable<Reservation>> GetReservationByResourceIdAfterTodayAsync(int resourceId)
    {
        return await GetOrSetCacheAsync(
            BuildKeyHelper.BuildKeyByTypeIdAndValue(typeof(Resource), resourceId, typeof(Reservation)),
            () => _repository.GetReservationByUserIdFromToday(resourceId)
        );
    }

    
    public async Task<IEnumerable<Reservation>> GetReservationByTypeIdAfterTodayAsync(int typeId)
    {
        return await GetOrSetCacheAsync(
            BuildKeyHelper.BuildKeyByTypeIdAndValue(typeof(Resource), typeId, typeof(Reservation)),
            () =>   _repository.GetReservationByTypeIdAfterTodayAsync(typeId)
        );
    }
    
    private async Task<IEnumerable<Reservation>> GetOrSetCacheAsync(string redisKey, Func<Task<IEnumerable<Reservation>>> getFromDb)
    {
        var redisValue = await _redisService.GetAsync(redisKey);
        if (redisValue != null)
            return JsonConvert.DeserializeObject<IEnumerable<Reservation>>(redisValue) ?? new List<Reservation>();

        var result = await getFromDb();
        await _redisService.SetAsync(redisKey, JsonConvert.SerializeObject(result));
        return result;
    }

}