using Newtonsoft.Json;
using ReservationManager.Cache.Helper;
using ReservationManager.Cache.Redis;
using ReservationManager.Cache.Repositories;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.DomainModel.Operation;
using ReservationManager.Persistence.Repositories;

public class ClosingCalendarCachedRepository : CrudBaseEntityCacheRepository<ClosingCalendar>, IClosingCalendarRepository
{
    private readonly ClosingCalendarRepository _repository;
    private readonly IRedisService _redisService;

    public ClosingCalendarCachedRepository(ClosingCalendarRepository repository, IRedisService redisService)
        : base(repository, redisService)
    {
        _repository = repository;
        _redisService = redisService;
    }

    public async Task<IEnumerable<ClosingCalendar>> GetAllFromToday()
    {
        var closingCalendars = (await _repository.GetAllFromToday()).ToList();
        await CacheClosingCalendars(closingCalendars);
        return closingCalendars;
    }

    public async Task<IEnumerable<ClosingCalendar>> GetFiltered(int? id, DateOnly? fromDate, DateOnly? toDate, int? resourceId, int? resourceTypeId)
    {
        var closingCalendars = (await _repository.GetFiltered(id, fromDate, toDate, resourceId, resourceTypeId)).ToList();
        await CacheClosingCalendars(closingCalendars);
        return closingCalendars;
    }

    public async Task<IEnumerable<ClosingCalendar>> BulkCreateEntitiesAsync(IEnumerable<ClosingCalendar> entities)
    {
        var created = (await _repository.BulkCreateEntitiesAsync(entities)).ToList();
        await CacheClosingCalendars(created);
        return created;
    }

    public async Task<IEnumerable<ClosingCalendar>> GetExistingClosingCalendars(IEnumerable<int> resourceIds, IEnumerable<DateOnly> days)
    {
        var existing = (await _repository.GetExistingClosingCalendars(resourceIds, days)).ToList();
        await CacheClosingCalendars(existing);
        return existing;
    }

    private async Task CacheClosingCalendars(IEnumerable<ClosingCalendar> calendars)
    {
        foreach (var calendar in calendars)
        {
            var redisKey = BuildKeyHelper.BuildKeyByTypeAndId(typeof(ClosingCalendar), calendar.Id);
            await _redisService.SetIfNotExistsAsync(redisKey, JsonConvert.SerializeObject(calendar));
        }
    }
}
