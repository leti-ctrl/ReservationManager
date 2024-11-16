using Microsoft.EntityFrameworkCore;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.DomainModel.Operation;
using ReservationManager.Persistence.Repositories.Base;

namespace ReservationManager.Persistence.Repositories
{
    public class ReservationRepository : CrudBaseEntityRepository<Reservation>, IReservationRepository
    {
        public ReservationRepository(ReservationManagerDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Reservation>> GetByDayAsync(DateOnly day)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Reservation>> GetByResourceAsync(int resourceId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Reservation>> GetByTypeAsync(string code)
        {
            return await Context.Set<Reservation>()
                                .Where(x => x.Type.Code == code)
                                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetByResourceDateTimeAsync(List<int> resourceIds,
            DateOnly startDate, DateOnly endDate, TimeSpan startTime, TimeSpan endTime)
        {
            var query = Context.Set<Reservation>().AsQueryable();
            //query = query.Where(x => resourceIds.Contains(x.ResourceId));
            query = query.Where(x => startDate == x.Day && x.Day == endDate);
            query = query.Where(x => x.Start == startTime && x.End == endTime);
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetByUserAsync(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
