using Mapster;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Interfaces;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.Core.Interfaces.Services;
using ReservationManager.Core.Interfaces.Validators;
using ReservationManager.DomainModel.Operation;

namespace ReservationManager.Core.Services;

public class ResourceFilterService : IResourceFilterService
{
    private readonly IResourceFilterValidator _resourceFilterValidator;
    private readonly IResourceReservedMapper _resourceReservedMapper;
    private readonly IResourceRepository _resourceRepository;
    private readonly IReservationRepository _reservationRepository;
    private readonly IClosingCalendarFilterService _closingCalendarFilterService;


    public ResourceFilterService(IClosingCalendarFilterService closingCalendarFilterService, 
        IResourceFilterValidator resourceFilterValidator, IResourceReservedMapper resourceReservedMapper, 
        IResourceRepository resourceRepository, IReservationRepository reservationRepository)
    {
        _closingCalendarFilterService = closingCalendarFilterService;
        _resourceFilterValidator = resourceFilterValidator;
        _resourceReservedMapper = resourceReservedMapper;
        _resourceRepository = resourceRepository;
        _reservationRepository = reservationRepository;
    }
    
    public async Task<IEnumerable<ResourceDto>> GetFilteredResources(ResourceFilterDto resourceFilters)
    {
        var validationResult = await _resourceFilterValidator.ValidateAsync(resourceFilters);
        if (!validationResult.IsValid)
        {
            var errorMsg = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new ArgumentException(errorMsg);
        }

        return await RetrieveFilteredResources(resourceFilters);
    }

    private async Task<IEnumerable<ResourceDto>> RetrieveFilteredResources(ResourceFilterDto resourceFilters)
    {
        var resourceList = await GetBaseFilteredResources(resourceFilters);
        if (!resourceList.Any())
            return Enumerable.Empty<ResourceDto>();

        if (AreDateTimeFiltersApplied(resourceFilters))
        {
            var closed = (await _closingCalendarFilterService.GetFiltered(new ClosingCalendarFilterDto()
            {
                StartDay = resourceFilters.Day,
                EndDay = resourceFilters.Day,
                RescourceId = resourceFilters.ResourceId,
                ResourceTypeId = resourceFilters.TypeId
            })).ToList();
            var reservationFilteredList = await GetReservationFilteredResources(resourceList, resourceFilters);
            if (reservationFilteredList.Any() || closed.Any())
                return _resourceReservedMapper.Map(resourceList, reservationFilteredList, closed.ToList());
        }

        return resourceList.Select(x => x.Adapt<ResourceDto>());
    }
    
    private async Task<List<Resource>> GetBaseFilteredResources(ResourceFilterDto resourceFilters)
    {
        return (await _resourceRepository.GetFiltered(resourceFilters.TypeId, resourceFilters.ResourceId)).ToList();
    }

    private static bool AreDateTimeFiltersApplied(ResourceFilterDto resourceFilters)
    {
        return resourceFilters is { Day: not null, TimeFrom: not null, TimeTo: not null };
    }
    
    private async Task<List<Reservation>> GetReservationFilteredResources(IEnumerable<Resource> resourceList,
        ResourceFilterDto resourceFilters)
    {
        var resourceIds = resourceList.Select(x => x.Id).ToList();
        return (await _reservationRepository.GetReservationByResourceDateTimeAsync(
            resourceIds,
            resourceFilters.Day!.Value,
            resourceFilters.TimeFrom!.Value,
            resourceFilters.TimeTo!.Value)).ToList();
    }
}