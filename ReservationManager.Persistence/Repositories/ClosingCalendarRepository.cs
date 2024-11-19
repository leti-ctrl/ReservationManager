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
                                .Where(x => x.Day == null || 
                                                          x.Day >= DateOnly.FromDateTime(DateTime.Now))
                                .ToListAsync();
        }

        public async Task<IEnumerable<ClosingCalendar>> GetByTypeId(int typeId)
        {
            return await Context.Set<ClosingCalendar>()
                                .ToListAsync();    
        }
    }
}
