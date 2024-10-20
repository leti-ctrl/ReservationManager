using Microsoft.EntityFrameworkCore;
using ReservationManager.DomainModel.Operation;
using ReservationManager.Persistence.Interfaces;
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

        public async Task<IEnumerable<Reservation>> GetByUserAsync(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
