﻿using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<Reservation>> GetReservationByResourceDateTimeAsync(List<int> resourceIds,
            DateOnly startDate, TimeOnly startTime, TimeOnly endTime)
        {
            return await Context.Set<Reservation>()
                                .Include(r => r.Resource)
                                .Where(x => resourceIds.Contains(x.Resource.Id))
                                .Where(x => startDate == x.Day)
                                .Where(x => x.Start >= startTime && x.End <= endTime)
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

        public async Task<IEnumerable<Reservation>> GetByUserAsync(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
