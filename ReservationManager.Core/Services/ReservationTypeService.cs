using FluentValidation.Results;
using Mapster;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Exceptions;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.Core.Interfaces.Services;
using ReservationManager.Core.Interfaces.Validators;
using ReservationManager.Core.Validators;
using ReservationManager.DomainModel.Meta;

namespace ReservationManager.Core.Services
{
    public class ReservationTypeService : IReservationTypeService
    {
        private readonly IReservationTypeRepository _reservationTypeRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IReservationTypeValidator _reservationTypeValidator;

        public ReservationTypeService(IReservationTypeRepository reservationTypeRepository, 
            IReservationRepository reservationRepository, IReservationTypeValidator reservationTypeValidator)
        {
            _reservationTypeRepository = reservationTypeRepository;
            _reservationRepository = reservationRepository;
            _reservationTypeValidator = reservationTypeValidator;
        }

        public async Task<IEnumerable<ReservationTypeDto>> GetAllReservationType()
        {
            var reservationTypes = await _reservationTypeRepository.GetAllTypesAsync();
           
            return reservationTypes.Select(rt => rt.Adapt<ReservationTypeDto>())
                .OrderByDescending(t => t.End - t.Start);
        }

        public async Task<ReservationTypeDto> CreateReservationType(UpsertReservationTypeDto request)
        {
            var toCreate = request.Adapt<ReservationType>();
            await ValidateModel(toCreate);
            await ValidateCode(0, toCreate);

            var newReservationType = await _reservationTypeRepository.CreateTypeAsync(toCreate);

            return newReservationType.Adapt<ReservationTypeDto>();
        }

        public async Task<ReservationTypeDto?> UpdateReservationType(int id, UpsertReservationTypeDto model)
        {
            var existingReservation = await _reservationTypeRepository.GetTypeById(id);
            if (existingReservation == null)
                return null;
            
            var toUpdate = model.Adapt<ReservationType>();
            await ValidateModel(toUpdate);
            await ValidateCode(id, toUpdate);

            toUpdate.Id = id;
            
            var updated = await _reservationTypeRepository.UpdateTypeAsync(toUpdate);

            return updated.Adapt<ReservationTypeDto>();
        }

        private async Task ValidateCode(int id, ReservationType toUpdate)
        {
            var codeAlreadyExists = await _reservationTypeRepository.GetByCodeAsync(toUpdate.Code);
            if(codeAlreadyExists != null && codeAlreadyExists?.Id != id)
                throw new InvalidCodeTypeException($"Reservation type with code {toUpdate.Code} already exists");
        }

        private async Task ValidateModel(ReservationType toUpdate)
        {
            var validationResult = await _reservationTypeValidator.ValidateAsync(toUpdate);
            if (!validationResult.IsValid)
            {
                var errorMsg = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new ArgumentException(errorMsg);
            }
        }

        public async Task DeleteReservationType(int id)
        {
            var toDelete = await _reservationTypeRepository.GetTypeById(id)
                ?? throw new EntityNotFoundException($"Reservation type with id {id} not found");
            
            var exists = await _reservationRepository.GetReservationByTypeIdAfterTodayAsync(id);
            if (exists.Any())
                throw new DeleteNotPermittedException($"Cannot delete {toDelete.Code} because exits future reservations with this type");

            await _reservationTypeRepository.DeleteTypeAsync(toDelete);
        }
    }
}