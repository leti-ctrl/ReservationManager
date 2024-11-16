using Mapster;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Exceptions;
using ReservationManager.Core.Interfaces;
using ReservationManager.Core.Validators;
using ReservationManager.DomainModel.Meta;
using ReservationManager.Persistence.Interfaces;
using ReservationManager.Persistence.Repositories;

namespace ReservationManager.Core.Services
{
    public class ReservationTypeService : IReservationTypeService
    {
        private readonly IReservationTypeRepository _reservationTypeRepository;
        private readonly IReservationRepository _reservationRepository;

        public ReservationTypeService(IReservationTypeRepository reservationTypeRepository, IReservationRepository reservationRepository)
        {
            _reservationTypeRepository = reservationTypeRepository;
            _reservationRepository = reservationRepository;
        }

        public async Task<IEnumerable<ReservationTypeDto>> GetAllReservationType()
        {
            var reservationTypes = await _reservationTypeRepository.GetAllTypesAsync();
            if (reservationTypes == null)
                return Enumerable.Empty<ReservationTypeDto>();

            return reservationTypes.Select(rt => rt.Adapt<ReservationTypeDto>());
        }

        public async Task<ReservationTypeDto> CreateReservationType(string code, string start, string end)
        {
            TimeOnly formattedStart, formattedEnd;
            ValidateInputs(start, end, out formattedStart, out formattedEnd);

            var model = new ReservationType
            {
                Code = code,
                Start = formattedStart,
                End = formattedEnd
            };

            var newReservationType = await _reservationTypeRepository.CreateTypeAsync(model);

            return newReservationType.Adapt<ReservationTypeDto>();
        }

        public async Task<ReservationTypeDto> UpdateReservationType(int id, string code, string start, string end)
        {
            TimeOnly formattedStart, formattedEnd;
            ValidateInputs(start, end, out formattedStart, out formattedEnd);

            var oldReseationType = await _reservationTypeRepository.GetTypeById(id)
                ?? throw new EntityNotFoundException($"Rervation type with id {id} not found");
            oldReseationType.Code = code;
            oldReseationType.Start = formattedStart;
            oldReseationType.End = formattedEnd;

            var updated = await _reservationTypeRepository.UpdateTypeAsync(oldReseationType);
            return updated.Adapt<ReservationTypeDto>();

        }

        public async Task DeleteReservationType(int id)
        {
            var toDelete = await _reservationTypeRepository.GetTypeById(id)
                ?? throw new EntityNotFoundException($"Rervation type with id {id} not found");
            
            var exists = await _reservationRepository.GetByTypeAsync(toDelete.Code);
            if (exists.Any() && 
                exists.Where(x => x.Day >= DateOnly.FromDateTime(DateTime.Now)).Any())
                throw new DeleteNotPermittedException($"Cannot delete {toDelete.Code} because exits future reservations with this type");

            await _reservationTypeRepository.DeleteTypeAsync(toDelete);
        }

        private static void ValidateInputs(string start, string end, out TimeOnly formattedStart, out TimeOnly formattedEnd)
        {
            formattedStart = start.StringToTimeOnly() ??
                            throw new TimeOnlyException("Start time is in the wrong format. Try with hh:mm:ss");
            formattedEnd = end.StringToTimeOnly() ??
                            throw new TimeOnlyException("End time is in the wrong format. Try with hh:mm:ss");
        }
    }
}
