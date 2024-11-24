﻿using ReservationManager.Core.Interfaces.Repositories.Base;
using ReservationManager.DomainModel.Operation;

namespace ReservationManager.Core.Interfaces.Repositories
{
    public interface IReservationRepository : ICrudBaseEntityRepository<Reservation>
    {
        Task<IEnumerable<Reservation>> GetByDayAsync(DateOnly day);
        Task<IEnumerable<Reservation>> GetByResourceAsync(int resourceId);
        Task<IEnumerable<Reservation>> GetByUserAsync(int userId);
        Task<IEnumerable<Reservation>> GetByTypeAsync(string code);
        Task<IEnumerable<Reservation>> GetReservationByResourceDateTimeAsync(List<int> resourceIds, DateOnly startDate,
            TimeOnly startTime, TimeOnly endTime);
        Task<IEnumerable<Reservation>> GetReservationByResourceIdAfterTodayAsync(int resourceId);
    }
}
