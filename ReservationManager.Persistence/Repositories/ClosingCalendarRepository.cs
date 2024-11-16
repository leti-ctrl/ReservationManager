using Microsoft.EntityFrameworkCore;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.DomainModel.Operation;
using ReservationManager.Persistence.Repositories.Base;

namespace ReservationManager.Persistence.Repositories
{
    public class ClosingCalendarRepository : CrudBaseEntityRepository<ClosingCalendar>,
        IClosingCalendarRepository
    {
        public ClosingCalendarRepository(ReservationManagerDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<ClosingCalendar>> GetByDateRange(DateOnly startDate, DateOnly endDate)
        {
            return await Context.Set<ClosingCalendar>()
                                .Where(x => x.StartDate >= startDate && x.EndDate <= endDate)
                                .ToListAsync();
        }

        public async Task<IEnumerable<ClosingCalendar>> GetClosingDateIntersection(DateOnly start, DateOnly end, int typeId)
        {
            return await Context.Set<ClosingCalendar>()
                                .Where(x => x.StartDate <= end && x.EndDate >= start)
                                .ToListAsync();
        }

        public async Task<IEnumerable<ClosingCalendar>> GetAllTimetableFromToday()
        {
            return await Context.Set<ClosingCalendar>()
                                .Where(x => x.StartDate == null || 
                                                          x.StartDate >= DateOnly.FromDateTime(DateTime.Now))
                                .ToListAsync();
        }

        public async Task<IEnumerable<ClosingCalendar>> GetByTypeId(int typeId)
        {
            return await Context.Set<ClosingCalendar>()
                                .ToListAsync();    
        }
        
        public async Task<IEnumerable<ClosingCalendar>> GetTimeReductionIntersection(DateOnly startDate, 
            DateOnly endDate, TimeOnly startTime, TimeOnly endTime, int typeId) 
        {
            return await Context.Set<ClosingCalendar>()
                                .Where(x => x.StartDate <= endDate && x.EndDate >= startDate)
                                .Where(x => x.StartTime >= endTime && x.EndTime <= startTime)
                                .ToListAsync();
        }
    }
}
