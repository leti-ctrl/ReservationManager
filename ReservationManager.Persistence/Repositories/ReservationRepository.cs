using ReservationManager.DomainModel.Operation;
using ReservationManager.Persistence.Interfaces;
using ReservationManager.Persistence.Repositories.Base;

namespace ReservationManager.Persistence.Repositories
{
    public class ReservationRepository : CrudEntityBaseRepository<Reservation>, IReservationRepository
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
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Reservation>> GetByUserAsync(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
