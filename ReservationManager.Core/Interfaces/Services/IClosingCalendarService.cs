using ReservationManager.Core.Dtos;

namespace ReservationManager.Core.Interfaces.Services
{
    public interface IClosingCalendarService
    {
        Task<IEnumerable<ClosingCalendarDto>> GetAll();
        Task<IEnumerable<ClosingCalendarDto>> GetByTypeId(int typeId);
        Task<IEnumerable<ClosingCalendarDto>> GetByDateRange(DateOnly startDate, DateOnly endDate);
        Task<ClosingCalendarDto> Create(UpsertClosingCalendarDto entity);
        Task<ClosingCalendarDto> Update(int id, UpsertClosingCalendarDto entity);
        Task Delete(int id);
    }
}
