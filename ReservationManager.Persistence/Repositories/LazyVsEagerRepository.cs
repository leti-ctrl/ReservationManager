using Microsoft.EntityFrameworkCore;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.DomainModel.Dtos;
using ReservationManager.DomainModel.Operation;

namespace ReservationManager.Persistence.Repositories;

public class LazyVsEagerRepository : ILazyVsEagerRepository
{
    private readonly ReservationManagerDbContext _dbContext;
    
    public LazyVsEagerRepository(ReservationManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IEnumerable<ResourceRepoDto>> EagerGetAllResourcesAsDtoAsync()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        var resources = await _dbContext.Set<Resource>()
            .Include(r => r.Reservations)
            .Include(r => r.ClosingCalendars)
            .ToListAsync();

        var resourceDtos = resources.Select(r => new ResourceRepoDto
        {
            Id = r.Id,
            Description = r.Description,
            ResourceReservedDtos = r.ClosingCalendars
                .Where(c => c.Day > today)
                .Select(c => new ResourceReservedRepoDto
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
                        .Select(res => new ResourceReservedRepoDto
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

        return resourceDtos;
    }

    public async Task<IEnumerable<ResourceRepoDto>> LazyGetAllResourcesAsDtoAsync()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        var resources = await _dbContext.Set<Resource>().ToListAsync(); // Non includiamo esplicitamente Reservations e ClosingCalendars

        var resourceDtos = resources.Select(r => new ResourceRepoDto
        {
            Id = r.Id,
            Description = r.Description,
            ResourceReservedDtos = r.ClosingCalendars
                .Where(c => c.Day > today)
                .Select(c => new ResourceReservedRepoDto
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
                        .Select(res => new ResourceReservedRepoDto
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

        return resourceDtos;
    }

}