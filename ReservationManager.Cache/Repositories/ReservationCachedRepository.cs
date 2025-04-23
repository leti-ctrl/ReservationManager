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
        return await SetCacheAsync(
            () => _repository.GetReservationByUserIdFromToday(userId)
        );
    }


    public async Task<IEnumerable<Reservation>> GetReservationByResourceDateTimeAsync(List<int> resourceIds, DateOnly startDate, TimeOnly startTime, TimeOnly endTime)
    {
        return await SetCacheAsync(
            () =>  _repository.GetReservationByResourceDateTimeAsync(resourceIds, startDate, startTime, endTime)
        );
    }

    public async Task<IEnumerable<Reservation>> GetReservationByResourceIdAfterTodayAsync(int resourceId)
    {
        return await SetCacheAsync(
            () => _repository.GetReservationByUserIdFromToday(resourceId)
        );
    }

    
    public async Task<IEnumerable<Reservation>> GetReservationByTypeIdAfterTodayAsync(int typeId)
    {
        return await SetCacheAsync(
            () =>   _repository.GetReservationByTypeIdAfterTodayAsync(typeId)
        );
    }
    
    private async Task<IEnumerable<Reservation>> SetCacheAsync(Func<Task<IEnumerable<Reservation>>> getFromDb)
    {
        var reservationList = (await getFromDb()).ToList();

        foreach (var reservation in reservationList)
        {
            var redisKey = BuildKeyHelper.BuildKeyByTypeAndId(typeof(Reservation), reservation.Id);
            await _redisService.RefreshOrAddValueAsync(redisKey, JsonConvert.SerializeObject(reservation));
        }
        
        return reservationList;
    }

}