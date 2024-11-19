using ReservationManager.DomainModel.Operation;
using ReservationManager.Core.Interfaces.Repositories.Base;

namespace ReservationManager.Core.Interfaces.Repositories
{
    public interface IClosingCalendarRepository : ICrudBaseEntityRepository<ClosingCalendar>
    {
        Task<IEnumerable<ClosingCalendar>> GetAllFromToday();
        Task<IEnumerable<ClosingCalendar>> GetByTypeId(int typeId);
    }
}
