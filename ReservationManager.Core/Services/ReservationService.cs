﻿using Mapster;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Exceptions;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.Core.Interfaces.Services;
using ReservationManager.DomainModel.Operation;
namespace ReservationManager.Core.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IReservationTypeRepository _reservationTypeRepository;

        public ReservationService(IReservationRepository reservationRepository, IReservationTypeRepository reservationTypeRepository)
        {
            _reservationRepository = reservationRepository;
            _reservationTypeRepository = reservationTypeRepository;
        }

        public async Task<ReservationDto> CreateReservation(UpsertReservationDto reservation)
        {
            var type = await _reservationTypeRepository.GetTypeById(reservation.TypeId)
                ?? throw new InvalidCodeTypeException($"Reservation type {reservation.TypeId} not valid");

            var toCreate = reservation.Adapt<Reservation>();
            toCreate.TypeId = type.Id;
            var created = await _reservationRepository.CreateEntityAsync(toCreate);

            var toRet = await _reservationRepository.GetEntityByIdAsync(created.Id);
            return toRet.Adapt<ReservationDto>();
        }

        public Task DeleteReservation(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ReservationDto> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ReservationDto>> GetUserReservation(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<ReservationDto> UpdateReservation(UpsertReservationDto reservation)
        {
            throw new NotImplementedException();
        }
    }
}
