using Mapster;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Exceptions;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.Core.Interfaces.Services;
using ReservationManager.DomainModel.Operation;

namespace ReservationManager.Core.Services
{
    public class ClosingCalendarService : IClosingCalendarService
    {
        private readonly IClosingCalendarRepository _closingCalendarRepository;

        public ClosingCalendarService(IClosingCalendarRepository closingCalendarRepository)
        {
            _closingCalendarRepository = closingCalendarRepository;
        }

        public async Task<IEnumerable<ClosingCalendarDto>> GetByDateRange(DateOnly startDate, DateOnly endDate)
        {
            var list = await _closingCalendarRepository.GetByDateRange(startDate, endDate);
            
            return list.Select(t => t.Adapt<ClosingCalendarDto>());
        }

        public async Task<ClosingCalendarDto> Create(UpsertClosingCalendarDto entity)
        {
            var model = entity.Adapt<ClosingCalendar>();

            var created = await _closingCalendarRepository.CreateEntityAsync(model);
            
            return created.Adapt<ClosingCalendarDto>();
        }

        public async Task<ClosingCalendarDto> Update(int id, UpsertClosingCalendarDto entity)
        {
            var timetableList = await _closingCalendarRepository.GetEntityByIdAsync(id) ??
                                throw new EntityNotFoundException($"Timetable {id} does not exist.");
            

            
            var model = entity.Adapt<ClosingCalendar>();
            model.Id = id;
            
            var updated = await _closingCalendarRepository.UpdateEntityAsync(model);
            
            return updated.Adapt<ClosingCalendarDto>();
        }

        public async Task Delete(int id)
        {
            var entity = await _closingCalendarRepository.GetEntityByIdAsync(id) ??
                         throw new EntityNotFoundException($"Timetable {id} not found.");
            
            await _closingCalendarRepository.DeleteEntityAsync(entity);
        }

        public async Task<IEnumerable<ClosingCalendarDto>> GetAll()
        {
            var timetableList = (await _closingCalendarRepository.GetAllTimetableFromToday()).ToList();
            
            return !timetableList.Any() 
                ? Enumerable.Empty<ClosingCalendarDto>() 
                : timetableList.Select(x => x.Adapt<ClosingCalendarDto>());
        }

        public async Task<IEnumerable<ClosingCalendarDto>> GetByTypeId(int typeId)
        {
            var timetableList = await _closingCalendarRepository.GetByTypeId(typeId);
            
            return timetableList.Select(item => item.Adapt<ClosingCalendarDto>());
        }
    }
}
