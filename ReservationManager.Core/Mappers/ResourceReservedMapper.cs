using Mapster;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Interfaces;
using ReservationManager.DomainModel.Operation;

namespace ReservationManager.Core.Mappers;

public class ResourceReservedMapper : IResourceReservedMapper
{
    public IEnumerable<ResourceDto> Map(IEnumerable<Resource> resources, IEnumerable<Reservation> reservations)
    {
        // Group reservations by ResourceId for more efficient lookup
        var reservationsByResourceId = reservations
            .GroupBy(r => r.ResourceId)
            .ToDictionary(g => g.Key, g => g.Select(r => new ResourceReservedDto
            {
                Day = r.Day,
                TimeStart = r.Start,
                TimeEnd = r.End
            }).ToList());

        // Map each resource to ResourceDto, including its reservations if any
        var model = resources.Select(resource =>
        {
            var resDto = resource.Adapt<ResourceDto>();
        
            // Get reservations for the current resource, if any
            resDto.ResourceReservedDtos = reservationsByResourceId.TryGetValue(resource.Id, out var reservedList)
                ? reservedList
                : new List<ResourceReservedDto>();

            return resDto;
        });

        return model;
    }

}