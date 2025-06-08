using Microsoft.EntityFrameworkCore;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.DomainModel.Daos;
using ReservationManager.DomainModel.Operation;

namespace ReservationManager.Persistence.Repositories;

public class LazyVsEagerRepository : ILazyVsEagerRepository
{
    private readonly ReservationManagerDbContext _dbContext;
    
    public LazyVsEagerRepository(ReservationManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IEnumerable<ResourceRepoDao>> EagerGetAllBusyResourcesFromTodayAsync()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        var resources = await _dbContext.Set<Resource>()
            .Include(r => r.Reservations.Where(x => x.Day > today))
            .Include(r => r.ClosingCalendars.Where(x => x.Day > today))
            .ToListAsync();

        var resourceDaos = resources.Select(r => new ResourceRepoDao
        {
            Id = r.Id,
            Description = r.Description,
            ResourceReservedDtos = r.ClosingCalendars
                .Where(c => c.Day > today)
                .Select(c => new ResourceReservedRepoDao
                {
                    IsClosed = true,
                    Day = c.Day,
                    TimeStart = null,
                    TimeEnd = null,
                    ReservationId = null
                })
                .Concat(
                    r.Reservations
                        .Where(res => res.Day > today)
                        .Select(res => new ResourceReservedRepoDao
                        {
                            IsClosed = false,
                            Day = res.Day,
                            TimeStart = res.Start,
                            TimeEnd = res.End,
                            ReservationId = res.Id
                        })
                )
                .OrderBy(x => x.Day)
                .ToList()
        }).ToList();

        return resourceDaos;
    }

    public async Task<IEnumerable<ResourceRepoDao>> LazyGetAllBusyResourcesFromTodayAsync()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        // Non includiamo esplicitamente Reservations e ClosingCalendars
        var resources = await _dbContext.Set<Resource>().ToListAsync(); 

        var resourceDaos = resources.Select(r => new ResourceRepoDao
        {
            Id = r.Id,
            Description = r.Description,
            ResourceReservedDtos = r.ClosingCalendars
                .Where(c => c.Day > today)
                .Select(c => new ResourceReservedRepoDao
                {
                    IsClosed = true,
                    Day = c.Day,
                    TimeStart = null,
                    TimeEnd = null,
                    ReservationId = null
                })
                .Concat(
                    r.Reservations
                        .Where(res => res.Day > today)
                        .Select(res => new ResourceReservedRepoDao
                        {
                            IsClosed = false,
                            Day = res.Day,
                            TimeStart = res.Start,
                            TimeEnd = res.End,
                            ReservationId = res.Id
                        })
                )
                .OrderBy(x => x.Day)
                .ToList()
        }).ToList();

        return resourceDaos;
    }

    public async Task<IEnumerable<ResourceRepoDao>> EagerGetBusyResourceByResourceIdAndDayAsync(int resourceId, DateOnly day)
    {
        var resources = await _dbContext.Set<Resource>()
            .Where(r => r.Id == resourceId)
            .Include(r => r.Reservations
                .Where(rez => rez.Day == day && rez.ResourceId == resourceId))
            .Include(r => r.ClosingCalendars
                .Where(cl => cl.Day == day && cl.ResourceId == resourceId))
            .ToListAsync();

        var resourceDaos = resources.Select(r => new ResourceRepoDao
        {
            Id = r.Id,
            Description = r.Description,
            ResourceReservedDtos = r.ClosingCalendars
                .Select(c => new ResourceReservedRepoDao
                {
                    IsClosed = true,
                    Day = c.Day,
                    TimeStart = null,
                    TimeEnd = null,
                    ReservationId = null
                })
                .Concat(
                    r.Reservations
                        .Select(res => new ResourceReservedRepoDao
                        {
                            IsClosed = false,
                            Day = res.Day,
                            TimeStart = res.Start,
                            TimeEnd = res.End,
                            ReservationId = res.Id
                        })
                )
                .OrderBy(x => x.Day)
                .ToList()
        }).ToList();

        return resourceDaos;
    }

    public async Task<IEnumerable<ResourceRepoDao>> LazyGetBusyResourceByResourceIdAndDayAsync(int resourceId, DateOnly day)
    {
        var resources = await _dbContext.Set<Resource>()
            .Where(r => r.Id == resourceId)
            .ToListAsync(); 

        var resourceDao = resources.Select(r => new ResourceRepoDao
        {
            Id = r.Id,
            Description = r.Description,
            ResourceReservedDtos = r.ClosingCalendars
                .Where(c => c.Day == day)
                .Select(c => new ResourceReservedRepoDao
                {
                    IsClosed = true,
                    Day = c.Day,
                    TimeStart = null,
                    TimeEnd = null,
                    ReservationId = null
                })
                .Concat(
                    r.Reservations
                        .Where(res => res.Day == day)
                        .Select(res => new ResourceReservedRepoDao
                        {
                            IsClosed = false,
                            Day = res.Day,
                            TimeStart = res.Start,
                            TimeEnd = res.End,
                            ReservationId = res.Id
                        })
                )
                .OrderBy(x => x.Day)
                .ToList()
        }).ToList();

        return resourceDao;
    }

    public async Task<IEnumerable<ResourceRepoDao>> EagerGetBusyResourceByResourceTypeIdAndDayAsync(int resourceTypeId,
        DateOnly day)
    {
        var resources = await _dbContext.Set<Resource>()
            .Where(r => r.TypeId == resourceTypeId)
            .Include(r => r.Reservations
                .Where(rez => rez.Day == day))
            .Include(r => r.ClosingCalendars
                .Where(cl => cl.Day == day))
            .ToListAsync();

        var resourceDaos = resources.Select(r => new ResourceRepoDao
        {
            Id = r.Id,
            Description = r.Description,
            ResourceReservedDtos = r.ClosingCalendars
                .Select(c => new ResourceReservedRepoDao
                {
                    IsClosed = true,
                    Day = c.Day,
                    TimeStart = null,
                    TimeEnd = null,
                    ReservationId = null
                })
                .Concat(
                    r.Reservations
                        .Select(res => new ResourceReservedRepoDao
                        {
                            IsClosed = false,
                            Day = res.Day,
                            TimeStart = res.Start,
                            TimeEnd = res.End,
                            ReservationId = res.Id
                        })
                )
                .OrderBy(x => x.Day)
                .ToList()
        }).ToList();

        return resourceDaos;
    }

    public async Task<IEnumerable<ResourceRepoDao>> LazyGetBusyResourceByResourceTypeIdAndDayAsync(int resourceTypeId,
        DateOnly day)
    {
        var resources = await _dbContext.Set<Resource>()
            .Where(r => r.TypeId == resourceTypeId)
            .ToListAsync(); 

        var resourceDao = resources.Select(r => new ResourceRepoDao
        {
            Id = r.Id,
            Description = r.Description,
            ResourceReservedDtos = r.ClosingCalendars
                .Where(c => c.Day == day)
                .Select(c => new ResourceReservedRepoDao
                {
                    IsClosed = true,
                    Day = c.Day,
                    TimeStart = null,
                    TimeEnd = null,
                    ReservationId = null
                })
                .Concat(
                    r.Reservations
                        .Where(res => res.Day == day)
                        .Select(res => new ResourceReservedRepoDao
                        {
                            IsClosed = false,
                            Day = res.Day,
                            TimeStart = res.Start,
                            TimeEnd = res.End,
                            ReservationId = res.Id
                        })
                )
                .OrderBy(x => x.Day)
                .ToList()
        }).ToList();

        return resourceDao;
    }
}