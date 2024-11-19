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
                                .Where(x => x.Day >= DateOnly.FromDateTime(DateTime.Now))
                                .ToListAsync();
        }

        public async Task<IEnumerable<ClosingCalendar>> GetFiltered(int? id, DateOnly? fromDate, DateOnly? toDate,
            int? resourceId, int? resourceTypeId)
        {
            var set = Context.Set<ClosingCalendar>()
                                                    .Include(x => x.Resource)
                                                    .AsQueryable();
            if(id != null)
                set = set.Where(c => c.Id == id);
            if (fromDate != null)
                set = set.Where(c => c.Day >= fromDate);
            if(toDate != null)
                set = set.Where(x => x.Day <= toDate);
            if(resourceId != null)
                set = set.Where(c => resourceId == c.ResourceId);
            if(resourceTypeId != null)
                set = set.Where(c => c.Resource.Type.Id == resourceTypeId);
            return await set.ToListAsync();
        }

        public async Task<IEnumerable<ClosingCalendar>> GetByTypeId(int typeId)
        {
            return await Context.Set<ClosingCalendar>()
                                .ToListAsync();    
        }
    }
}
