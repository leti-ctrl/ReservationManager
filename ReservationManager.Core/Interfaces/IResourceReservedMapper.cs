using ReservationManager.Core.Dtos;
using ReservationManager.DomainModel.Operation;

namespace ReservationManager.Core.Interfaces;

public interface IResourceReservedMapper
{
    IEnumerable<ResourceDto> Map(IEnumerable<Resource> resources, IEnumerable<Reservation> reservations);
}

