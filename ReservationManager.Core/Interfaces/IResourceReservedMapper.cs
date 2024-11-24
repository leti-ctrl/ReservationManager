using ReservationManager.Core.Dtos;
using ReservationManager.DomainModel.Operation;

namespace ReservationManager.Core.Interfaces;

public interface IResourceReservedMapper
{
    IEnumerable<ResourceDto> Map(List<Resource> resources, List<Reservation> reservations,
        List<ClosingCalendarDto> closingCalendar);
}

