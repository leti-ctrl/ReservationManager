using Mapster;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Exceptions;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.Core.Interfaces.Services;
using ReservationManager.DomainModel.Meta;

namespace ReservationManager.Core.Services
{
    public class TimetableTypeService : ITimetableTypeService
    {
        private readonly ITimetableTypeRepository _timetableTypeRepository;

        public TimetableTypeService(ITimetableTypeRepository timetableTypeRepository)
        {
            _timetableTypeRepository = timetableTypeRepository;
        }

        public async Task<IEnumerable<TimetableTypeDto>> GetAllTypes()
        {
            var toRet = await _timetableTypeRepository.GetAllTypesAsync() ??
                Enumerable.Empty<TimetableType>();

            return toRet.Select(x => x.Adapt<TimetableTypeDto>());
        }

        public async Task<TimetableTypeDto> GetById(int id)
        {
            var timetable = await _timetableTypeRepository.GetTypeById(id) ??
                throw new EntityNotFoundException($"TimetableType {id} not found.");
            return timetable.Adapt<TimetableTypeDto>(); 
        }
    }
}
