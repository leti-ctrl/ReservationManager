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

        public async Task<IEnumerable<ClosingCalendar>> GetAllFromToday()
        {
            return await Context.Set<ClosingCalendar>()
                                .Where(x => x.Day.CompareTo(DateOnly.FromDateTime(DateTime.Now)) >= 0)
                                .ToListAsync();
        }

        public async Task<IEnumerable<ClosingCalendar>> GetFiltered(int? id, DateOnly? fromDate, DateOnly? toDate,
            int? resourceId, int? resourceTypeId)
        {
            var set = Context.Set<ClosingCalendar>()
                                                    .Include(x => x.Resource)
                                                    .AsQueryable();
            if(id.HasValue && id.Value != 0)
                set = set.Where(c => c.Id == id.Value);
            if (fromDate.HasValue)
                set = set.Where(c => c.Day >= fromDate.Value);
            if(toDate.HasValue)
                set = set.Where(x => x.Day <= toDate.Value);
            if(resourceId.HasValue && resourceId.Value != 0)
                set = set.Where(c => resourceId.Value == c.ResourceId);
            if(resourceTypeId.HasValue && resourceTypeId.Value != 0)
                set = set.Where(c => c.Resource.Type.Id == resourceTypeId.Value);
            return await set.ToListAsync();
        }

        public async Task<IEnumerable<ClosingCalendar>> GetByTypeId(int typeId)
        {
            return await Context.Set<ClosingCalendar>()
                                .ToListAsync();    
        }
        
        public async Task<IEnumerable<ClosingCalendar>> GetExistingClosingCalendars(IEnumerable<int> resourceIds, IEnumerable<DateOnly> days)
        {
            return await Context.Set<ClosingCalendar>().AsQueryable()
                .Where(c => resourceIds.Any(id => id == c.ResourceId) && days.Contains(c.Day))
                .ToListAsync();
        }

        public async Task<IEnumerable<ClosingCalendar>> CreateEntitiesAsync(IEnumerable<ClosingCalendar> entities)
        {
            await Context.Set<ClosingCalendar>().AddRangeAsync(entities);
            await Context.SaveChangesAsync();
            return entities;
        }

    }
}
