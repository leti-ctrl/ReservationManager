using Newtonsoft.Json;
using ReservationManager.Cache.Helper;
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
        var closingCalendars = await _repository.GetAllFromToday();
        foreach (var closingCalendar in closingCalendars)
        {
            var redisKey = BuildKeyHelper.BuildKeyByTypeAndId(typeof(ClosingCalendar), closingCalendar.Id);
            await _redisService.SetIfNotExistsAsync(redisKey, JsonConvert.SerializeObject(closingCalendar));
        }
        return closingCalendars;
    }


    public async Task<IEnumerable<ClosingCalendar>> GetFiltered(int? id, DateOnly? fromDate, DateOnly? toDate, int? resourceId, int? resourceTypeId)
    {
        return await _repository.GetFiltered(id, fromDate, toDate, resourceId, resourceTypeId);
    }

    public async Task<IEnumerable<ClosingCalendar>> BulkCreateEntitiesAsync(IEnumerable<ClosingCalendar> entities)
    {
        var createdClosingCalendars = await _repository.BulkCreateEntitiesAsync(entities);

        foreach (var closingCalendar in createdClosingCalendars)
        {
            var redisKey = BuildKeyHelper.BuildKeyByTypeAndId(typeof(ClosingCalendar), closingCalendar.Id);
            await _redisService.SetIfNotExistsAsync(redisKey, JsonConvert.SerializeObject(closingCalendar));
        }

        return createdClosingCalendars;
    }

    public async Task<IEnumerable<ClosingCalendar>> GetExistingClosingCalendars(IEnumerable<int> resourceIds, IEnumerable<DateOnly> days)
    {
        return await _repository.GetExistingClosingCalendars(resourceIds, days);
    }
}