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

        
    }
}
