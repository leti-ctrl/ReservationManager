using Microsoft.EntityFrameworkCore;
using ReservationManager.Core.Exceptions;
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

        public override Task<IEnumerable<Reservation>> GetAllEntitiesAsync(CancellationToken cancellationToken = default)
        {
            throw new OperationNotPermittedException("Cannot retrieve all reservations.");
        }

        public async Task<IEnumerable<Reservation>> GetReservationByUserIdFromToday(int userId)
        {
            return await Context.Set<Reservation>()
                                .Include(r => r.Resource)
                                .Where(r => r.UserId == userId && r.Day >= DateOnly.FromDateTime(DateTime.Now))
                                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetReservationByResourceDateTimeAsync(List<int> resourceIds,
            DateOnly startDate, TimeOnly startTime, TimeOnly endTime)
        {
            return await Context.Set<Reservation>()
                                .Include(r => r.Resource)
                                .Where(x => resourceIds.Any(id => id == x.ResourceId))
                                .Where(x => startDate == x.Day)
                                .Where(x => x.Start < endTime && x.End > startTime)
                                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetReservationByResourceIdAfterTodayAsync(int resourceId)
        {
            return await Context.Set<Reservation>()
                                .Include(r => r.Resource)
                                .Where(x => resourceId == x.Resource.Id)
                                .Where(x => x.Day >= DateOnly.FromDateTime(DateTime.Now))
                                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetReservationByTypeIdAfterTodayAsync(int typeId)
        {
            return await Context.Set<Reservation>()
                .Where(x => typeId == x.TypeId
                            && x.Day >= DateOnly.FromDateTime(DateTime.Now))
                .ToListAsync();
        }
    }
}
