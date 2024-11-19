using ReservationManager.Core.Dtos;

namespace ReservationManager.Core.Interfaces.Services
{
    public interface IClosingCalendarService
    {
        Task<IEnumerable<ClosingCalendarDto>> GetAllFromToday();
        Task<IEnumerable<ClosingCalendarDto>> GetFiltered(ClosingCalendarFilterDto filter);
        Task<ClosingCalendarDto> Create(ClosingCalendarDto closingCalendarDto);
        Task<IEnumerable<ClosingCalendarDto>> CreateBucket(ClosingCalendarBucketDto closingCalendarBucketDto);
        Task<ClosingCalendarDto> Update(int id, ClosingCalendarDto closingCalendarDto);
        Task Delete(int id);
    }
}
