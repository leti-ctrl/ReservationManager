using ReservationManager.Cache.Redis;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.DomainModel.Operation;
using ReservationManager.Persistence.Repositories;

namespace ReservationManager.Cache.Repositories;

public class ClosingCalendarCachedRepository : CrudBaseEntityCacheRepository<ClosingCalendar>, IClosingCalendarRepository
{
    private readonly ClosingCalendarRepository _repository;
    private readonly IRedisService _redisService;
    
    public ClosingCalendarCachedRepository(ClosingCalendarRepository repository, IRedisService redisService) 
        : base(repository, redisService)
    {
        _redisService = redisService;
        _repository = repository;
    }

    public async Task<IEnumerable<ClosingCalendar>> GetAllFromToday()
    {
        return await _repository.GetAllFromToday();
    }

    public async Task<IEnumerable<ClosingCalendar>> GetFiltered(int? id, DateOnly? fromDate, DateOnly? toDate, int? resourceId, int? resourceTypeId)
    {
        return await _repository.GetFiltered(id, fromDate, toDate, resourceId, resourceTypeId);
    }

    public async Task<IEnumerable<ClosingCalendar>> BulkCreateEntitiesAsync(IEnumerable<ClosingCalendar> entities)
    {
        return await _repository.BulkCreateEntitiesAsync(entities);
    }

    public async Task<IEnumerable<ClosingCalendar>> GetExistingClosingCalendars(IEnumerable<int> resourceIds, IEnumerable<DateOnly> days)
    {
        return await _repository.GetExistingClosingCalendars(resourceIds, days);
    }
}