using Mapster;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Exceptions;
using ReservationManager.Core.Interfaces;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.Core.Interfaces.Services;
using ReservationManager.Core.Interfaces.Validators;
using ReservationManager.DomainModel.Operation;

namespace ReservationManager.Core.Services
{
    public class ResourceService : IResourceService
    {
        private readonly IResourceRepository _resourceRepository;
        private readonly IResourceValidator _resourceValidator;
        private readonly IResourceFilterValidator _resourceFilterValidator;
        private readonly IReservationRepository _reservationRepository;
        private readonly IResourceReservedMapper _resourceReservedMapper;

        public ResourceService(IResourceRepository resourceRepository, IResourceValidator resourceValidator,
            IResourceFilterValidator resourceFilterValidator, IReservationRepository reservationRepository,
            IResourceReservedMapper resourceReservedMapper)
        {
            _resourceRepository = resourceRepository;
            _resourceValidator = resourceValidator;
            _resourceFilterValidator = resourceFilterValidator;
            _reservationRepository = reservationRepository;
            _resourceReservedMapper = resourceReservedMapper;
        }

        public async Task<IEnumerable<ResourceDto>> GetAllResources()
        {
            var resources = (await _resourceRepository.GetAllEntitiesAsync()).ToList();

            return !resources.Any()
                ? Enumerable.Empty<ResourceDto>()
                : resources.Select(x => x.Adapt<ResourceDto>());
        }

        public async Task<IEnumerable<ResourceDto>> GetFilteredResources(ResourceFilterDto resourceFilters)
        {
            // Validate filters
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
                var reservationFilteredList = await GetReservationFilteredResources(resourceList, resourceFilters);
                if (reservationFilteredList.Any())
                    return _resourceReservedMapper.Map(resourceList, reservationFilteredList);
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

        public async Task<ResourceDto> CreateResource(UpsertResourceDto resource)
        {
            var existType = await _resourceValidator.ValidateResourceType(resource.TypeId);
            if (!existType)
                throw new InvalidCodeTypeException($"Resource type {resource.TypeId} is not valid");

            var added = await _resourceRepository.CreateEntityAsync(resource.Adapt<Resource>());
            return added.Adapt<ResourceDto>();
        }

        public async Task<ResourceDto?> UpdateResource(int id, UpsertResourceDto resource)
        {
            var validateModel = await _resourceValidator.ValidateResourceType(resource.TypeId) 
                                    && await _resourceValidator.ExistingResouceId(id);
            if (!validateModel)
                return null;

            var toUpdate = resource.Adapt<Resource>();
            toUpdate.Id = id;

            var updated = await _resourceRepository.UpdateEntityAsync(toUpdate);

            return updated.Adapt<ResourceDto>();
        }

        public async Task DeleteResource(int id)
        {
            var entity = await _resourceRepository.GetEntityByIdAsync(id) 
                         ?? throw new EntityNotFoundException($"Resource with id {id} not found");
            
            var reservations = await _reservationRepository.GetReservationByResourceIdAfterTodayAsync(id);
            if(reservations.Any())
                throw new DeleteNotPermittedException("Resource cannot be deleted because of existing reservation");
            
            await _resourceRepository.DeleteEntityAsync(entity);
        }
    }
}