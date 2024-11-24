using Mapster;
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
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IResourceService _resourceService;
        private readonly IUpsertReservationValidator _upsertReservationValidator;
        private readonly IReservationTypeRepository _reservationTypeRepository;


        public ReservationService(IReservationRepository reservationRepository,
            IUpsertReservationValidator upsertReservationValidator, IResourceService resourceService,
            IReservationTypeRepository reservationTypeRepository)
        {
            _reservationRepository = reservationRepository;
            _upsertReservationValidator = upsertReservationValidator;
            _resourceService = resourceService;
            _reservationTypeRepository = reservationTypeRepository;
        }

        public async Task<IEnumerable<ReservationDto>> GetUserReservation(int userId)
        {
            var reservationList = await _reservationRepository.GetReservationByUserIdFromToday(userId);
            
            return reservationList.ToList().Select(x => x.Adapt<ReservationDto>());
        }

        public async Task<ReservationDto?> GetById(int id, int userId)
        {
            var reservation = await _reservationRepository.GetEntityByIdAsync(id);
            if (reservation == null)
                return null;
            
            if(reservation.UserId != userId)
                throw new OperationNotPermittedException("Cannot update because user does not belong to this reservation.");

            return reservation.Adapt<ReservationDto>();
        }

        
        public async Task<ReservationDto> CreateReservation(int userId, UpsertReservationDto reservation)
        {
            var rezType = await GetLegalReservationType(reservation);

            var toCreate = reservation.Adapt<Reservation>();
            toCreate.UserId = userId;
            await CheckResourceIsFreeOrOpen(rezType!, toCreate);

            var created = await _reservationRepository.CreateEntityAsync(toCreate);

            var toRet = await _reservationRepository.GetEntityByIdAsync(created.Id);
            return toRet.Adapt<ReservationDto>();
        }

        private async Task<ReservationType?> GetLegalReservationType(UpsertReservationDto reservation)
        {
            var rezType = await _reservationTypeRepository.GetTypeById(reservation.TypeId)
                ?? throw new InvalidCodeTypeException("No reservation type found");
            
            if (!_upsertReservationValidator.IsDateRangeValid(reservation, rezType))
                throw new ArgumentException("Invalid reservation");
            
            return rezType;
        }


        public async Task<ReservationDto?> UpdateReservation(int reservationId, int userId,
            UpsertReservationDto reservation)
        {
            var oldRez = await _reservationRepository.GetEntityByIdAsync(reservationId);
            if (oldRez == null)
                return null;
            if(oldRez.UserId != userId)
                throw new OperationNotPermittedException("Cannot update because user does not belong to this reservation.");
            
            var rezType =  await GetLegalReservationType(reservation);

            var toUpdate = reservation.Adapt<Reservation>();
            toUpdate.Id = reservationId;
            toUpdate.UserId = userId;
            await CheckResourceIsFreeOrOpen(rezType!, toUpdate);


            var updated = await _reservationRepository.UpdateEntityAsync(toUpdate);
            
            var toRet = await _reservationRepository.GetEntityByIdAsync(updated!.Id);
            return toRet.Adapt<ReservationDto>();
        }

        private async Task CheckResourceIsFreeOrOpen(ReservationType reservationType, Reservation reservation)
        {
            if (reservationType.Code != FixedReservationType.Customizable)
            {
                reservation.Start = reservationType.Start;
                reservation.End = reservationType.End;
            }
            
            var resourceReserved = (await _resourceService.GetFilteredResources(new ResourceFilterDto()
            {
                ResourceId = reservationType.Id,
                Day = reservation.Day,
                TimeFrom = reservation.Start,
                TimeTo = reservation.End,
            })).FirstOrDefault();
            if (resourceReserved == null)
                throw new ArgumentException("Invalid resource for reservation");
            if(resourceReserved.ResourceReservedDtos != null && resourceReserved.ResourceReservedDtos.Any())
                throw new ArgumentException("Invalid resource for reservation");
        }

        public async Task DeleteReservation(int id, int userId)
        {
            var toDelete = await _reservationRepository.GetEntityByIdAsync(id);
            if (toDelete == null)
                throw new EntityNotFoundException("Reservation not found.");
            
            if(toDelete.UserId != userId)
                throw new OperationNotPermittedException("Cannot update because user does not belong to this reservation.");
            
            await _reservationRepository.DeleteEntityAsync(toDelete);
        }
    }
}
