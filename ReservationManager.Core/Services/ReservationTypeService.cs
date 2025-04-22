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
    public class ReservationTypeService(
        IReservationTypeRepository reservationTypeRepository,
        IReservationRepository reservationRepository,
        IReservationTypeValidator reservationTypeValidator)
        : IReservationTypeService
    {
        public async Task<IEnumerable<ReservationTypeDto>> GetAllReservationType()
        {
            var reservationTypes = await reservationTypeRepository.GetAllTypesAsync();
           
            return reservationTypes.Select(rt => rt.Adapt<ReservationTypeDto>())
                .OrderByDescending(t => t.End - t.Start);
        }

        public async Task<ReservationTypeDto> CreateReservationType(UpsertReservationTypeDto request)
        {
            var toCreate = request.Adapt<ReservationType>();
            await ValidateModel(toCreate);
            await ValidateCode(0, toCreate);

            var newReservationType = await reservationTypeRepository.CreateTypeAsync(toCreate);

            return newReservationType.Adapt<ReservationTypeDto>();
        }

        public async Task<ReservationTypeDto?> UpdateReservationType(int id, UpsertReservationTypeDto model)
        {
            var existingReservation = await reservationTypeRepository.GetTypeById(id);
            if (existingReservation == null)
                return null;
            
            var toUpdate = model.Adapt<ReservationType>();
            await ValidateModel(toUpdate);
            await ValidateCode(id, toUpdate);

            toUpdate.Id = id;
            
            var updated = await reservationTypeRepository.UpdateTypeAsync(toUpdate);

            return updated.Adapt<ReservationTypeDto>();
        }

        private async Task ValidateCode(int id, ReservationType toUpdate)
        {
            var codeAlreadyExists = await reservationTypeRepository.GetTypeByCode(toUpdate.Code);
            if(codeAlreadyExists != null && codeAlreadyExists?.Id != id)
                throw new InvalidCodeTypeException($"Reservation type with code {toUpdate.Code} already exists");
        }

        private async Task ValidateModel(ReservationType toUpdate)
        {
            var validationResult = await reservationTypeValidator.ValidateAsync(toUpdate);
            if (!validationResult.IsValid)
            {
                var errorMsg = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new ArgumentException(errorMsg);
            }
        }

        public async Task DeleteReservationType(int id)
        {
            var toDelete = await reservationTypeRepository.GetTypeById(id)
                ?? throw new EntityNotFoundException($"Reservation type with id {id} not found");
            
            var exists = await reservationRepository.GetReservationByTypeIdAfterTodayAsync(id);
            if (exists.Any())
                throw new DeleteNotPermittedException($"Cannot delete {toDelete.Code} because exits future reservations with this type");

            await reservationTypeRepository.DeleteTypeAsync(toDelete);
        }
    }
}