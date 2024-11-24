using Mapster;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Interfaces;
using ReservationManager.DomainModel.Operation;

namespace ReservationManager.Core.Mappers;

public class ResourceReservedMapper : IResourceReservedMapper
{
    public IEnumerable<ResourceDto> Map(List<Resource> resources, List<Reservation> reservations,
        List<ClosingCalendarDto> closingCalendar)
    {
        var toRet = new List<ResourceDto>();
        foreach (var resource in resources)
        {
            var item = resource.Adapt<ResourceDto>();
            item.ResourceReservedDtos = new List<ResourceReservedDto>(); 
            
            foreach (var reservation in reservations.Where(r => r.ResourceId == resource.Id))
            {
                var reserved = new ResourceReservedDto
                {
                    IsClosed = false,
                    Day = reservation.Day,
                    TimeStart = reservation.Start,
                    TimeEnd = reservation.End
                };
                item.ResourceReservedDtos.Add(reserved);
            }

            foreach (var closingDay in closingCalendar.Where(x => x.ResourceId == resource.Id))
            {
                var closed = new ResourceReservedDto
                {
                    IsClosed = true,
                    Day = closingDay.Day,
                    TimeStart = TimeOnly.MinValue,
                    TimeEnd = TimeOnly.MaxValue
                };
                item.ResourceReservedDtos.Add(closed);
            }
            toRet.Add(item);
        }
        return toRet;
    }

}