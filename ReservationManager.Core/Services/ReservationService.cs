﻿using Mapster;
using ReservationManager.Core.Consts;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Exceptions;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.Core.Interfaces.Services;
using ReservationManager.Core.Interfaces.Validators;
using ReservationManager.DomainModel.Meta;
using ReservationManager.DomainModel.Operation;

namespace ReservationManager.Core.Services
{
    public class ReservationService(
        IReservationRepository reservationRepository,
        IUpsertReservationValidator upsertReservationValidator,
        IResourceService resourceService,
        IReservationTypeRepository reservationTypeRepository,
        IUserService userService)
        : IReservationService
    {
        public async Task<IEnumerable<ReservationDto>> GetUserReservation(SessionInfo session)
        {
            var user = await userService.GetUserByEmail(session.UserEmail);
            if (user is null)
                throw new OperationNotPermittedException("Cannot retrieve reservation because user does not exist");

            var reservationList = await reservationRepository.GetReservationByUserIdFromToday(user.Id);
            
            return reservationList.ToList().Select(x => x.Adapt<ReservationDto>());
        }

        public async Task<ReservationDto?> GetById(int id, SessionInfo session)
        {
            var user = await userService.GetUserByEmail(session.UserEmail);
            if (user is null)
                throw new OperationNotPermittedException("Cannot retrieve reservation because user does not exist");
            
            var reservation = await reservationRepository.GetEntityByIdAsync(id);
            if (reservation == null)
                return null;
            
            if(reservation.UserId != user.Id)
                throw new OperationNotPermittedException("Cannot retrieve reservation because user does not belong to this reservation.");

            return reservation.Adapt<ReservationDto>();
        }

        
        public async Task<ReservationDto> CreateReservation(SessionInfo session, UpsertReservationDto reservation)
        {
            var user = await userService.GetUserByEmail(session.UserEmail);
            if(user == null)
                throw new OperationNotPermittedException("Cannot create reservation because user does not exist.");

            var rezType = await GetLegalReservationType(reservation);
            var toCreate = reservation.Adapt<Reservation>();
            toCreate.UserId = user.Id;
            await CheckResourceIsFreeOrOpen(rezType!, toCreate);

            var created = await reservationRepository.CreateEntityAsync(toCreate);

            var toRet = await reservationRepository.GetEntityByIdAsync(created.Id);
            return toRet.Adapt<ReservationDto>();
        }

        private async Task<ReservationType?> GetLegalReservationType(UpsertReservationDto reservation)
        {
            var rezType = await reservationTypeRepository.GetTypeById(reservation.TypeId)
                ?? throw new InvalidCodeTypeException("No reservation type found");
            
            if (!upsertReservationValidator.IsDateRangeValid(reservation, rezType))
                throw new ArgumentException("Invalid reservation");
            
            return rezType;
        }


        public async Task<ReservationDto?> UpdateReservation(int reservationId, SessionInfo session,
            UpsertReservationDto reservation)
        {
            var user = await userService.GetUserByEmail(session.UserEmail);
            if(user == null)
                throw new OperationNotPermittedException("Cannot update reservation because user does not exist.");
            
            var oldRez = await reservationRepository.GetEntityByIdAsync(reservationId);
            if (oldRez == null)
                return null;
            if(oldRez.UserId != user.Id)
                throw new OperationNotPermittedException("Cannot update because user does not belong to this reservation.");
            
            var rezType =  await GetLegalReservationType(reservation);

            var toUpdate = reservation.Adapt<Reservation>();
            toUpdate.Id = reservationId;
            toUpdate.UserId = user.Id;
            await CheckResourceIsFreeOrOpen(rezType!, toUpdate);


            var updated = await reservationRepository.UpdateEntityAsync(toUpdate);
            
            var toRet = await reservationRepository.GetEntityByIdAsync(updated!.Id);
            return toRet.Adapt<ReservationDto>();
        }

        private async Task CheckResourceIsFreeOrOpen(ReservationType reservationType, Reservation reservation, int reservationIdToUpdate = 0)
        {
            if (reservationType.Code != FixedReservationType.Customizable)
            {
                reservation.Start = reservationType.Start;
                reservation.End = reservationType.End;
            }
            
            var resourceReserved = (await resourceService.GetFilteredResources(new ResourceFilterDto()
            {
                ResourceId = reservation.ResourceId,
                Day = reservation.Day,
                TimeFrom = reservation.Start,
                TimeTo = reservation.End,
            })).FirstOrDefault();
            if (resourceReserved == null)
                throw new ReservationException("Invalid resource for reservation");
            if (resourceReserved.ResourceReservedDtos != null)
            {
                if (resourceReserved.ResourceReservedDtos.Count == 1 &&
                    resourceReserved.ResourceReservedDtos.First().ReservationId == reservation.Id)
                    return;
                
                throw new ReservationException("Cannot reserve resource because is closed or is already reserved.");
            }
        }

        public async Task DeleteReservation(int id, SessionInfo session)
        {
            var user = await userService.GetUserByEmail(session.UserEmail);
            if(user == null)
                throw new OperationNotPermittedException("Cannot delete reservation because user does not exist.");

            
            var toDelete = await reservationRepository.GetEntityByIdAsync(id);
            if (toDelete == null)
                throw new EntityNotFoundException("Reservation not found.");
            
            if(toDelete.UserId != user.Id)
                throw new OperationNotPermittedException("Cannot delete because user does not belong to this reservation.");
            
            await reservationRepository.DeleteEntityAsync(toDelete);
        }
    }
}
