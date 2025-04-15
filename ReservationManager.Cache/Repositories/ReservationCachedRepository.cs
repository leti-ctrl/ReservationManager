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
        //var redisKey = BuildRedisKeyHelper.BuildKey(typeof(User), userId);
        return await _repository.GetReservationByUserIdFromToday(userId);
    }

    public async Task<IEnumerable<Reservation>> GetReservationByResourceDateTimeAsync(List<int> resourceIds, DateOnly startDate, TimeOnly startTime, TimeOnly endTime)
    {
        return await _repository.GetReservationByResourceDateTimeAsync(resourceIds, startDate, startTime, endTime);
    }

    public async Task<IEnumerable<Reservation>> GetReservationByResourceIdAfterTodayAsync(int resourceId)
    {
        return await _repository.GetReservationByResourceIdAfterTodayAsync(resourceId);
    }

    public async Task<IEnumerable<Reservation>> GetReservationByTypeIdAfterTodayAsync(int typeId)
    {
        return await _repository.GetReservationByTypeIdAfterTodayAsync(typeId);
    }
}