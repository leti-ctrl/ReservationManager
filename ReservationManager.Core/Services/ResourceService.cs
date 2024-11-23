using System.Globalization;
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

        public async Task<ResourceDto> GetResourceById(int resourceId)
        {
            var resource = await _resourceRepository.GetEntityByIdAsync(resourceId) 
                ?? throw new EntityNotFoundException($"Resource id {resourceId} not found");

            return resource.Adapt<ResourceDto>();
        }

        public async Task<IEnumerable<ResourceDto>> GetFilteredResources(ResourceFilterDto resourceFilters)
        {
            // Validate filters
            var validationResult = await _resourceFilterValidator.ValidateAsync(resourceFilters);
            if (!validationResult.IsValid)
            {
                var errorMsg = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new InvalidFiltersException(errorMsg);
            }

            // Retrieve filtered resources
            var resources = await _resourceRepository.GetFiltered(resourceFilters.TypeId, resourceFilters.ResourceId);
            var resourceList = resources.ToList();
            if (!resourceList.Any()) return Enumerable.Empty<ResourceDto>();

            // If date filters are applied, filter by reservations
            if (resourceFilters.DateFrom.HasValue && resourceFilters.DateTo.HasValue &&
                !string.IsNullOrEmpty(resourceFilters.TimeFrom)  &&  !string.IsNullOrEmpty(resourceFilters.TimeTo))
            {
                var timeStart = TimeOnly.Parse(resourceFilters.TimeFrom, CultureInfo.InvariantCulture);
                var timeEnd = TimeOnly.Parse(resourceFilters.TimeTo, CultureInfo.InvariantCulture);
                
                
                var reservationFilteredList = 
                    await _reservationRepository.GetByResourceDateTimeAsync(resourceList.Select(x => x.Id).ToList(), 
                        resourceFilters.DateFrom.Value, resourceFilters.DateTo.Value, timeStart, timeEnd);

                if (reservationFilteredList.Any())
                {
                    return _resourceReservedMapper.Map(resourceList, reservationFilteredList);
                }
            }

            // If no reservations are found or date filters are not applied, map resources directly
            return resourceList.Select(x => x.Adapt<ResourceDto>());
        }

        
        public async Task<ResourceDto> CreateResource(UpsertResourceDto resource)
        {
            var existType = await _resourceValidator.ValidateResourceType(resource.TypeId);
            if(!existType)
                throw new InvalidCodeTypeException($"Resource type {resource.TypeId} is not valid");
            
            var added = await _resourceRepository.CreateEntityAsync(resource.Adapt<Resource>());
            return added.Adapt<ResourceDto>();
        }

        public async Task<ResourceDto> UpdateResource(int id, UpsertResourceDto resource)
        {
            var existType = await _resourceRepository.GetEntityByIdAsync(id) ??
                            throw new EntityNotFoundException($"Resource id {id} not found");
            if(existType.TypeId != resource.TypeId)
                throw new UpdateNotPermittedException($"Resource type {resource.TypeId} does not match the previous one.");

            var toUpdate = resource.Adapt<Resource>();
            toUpdate.Id = id;
            
            var updated = await _resourceRepository.UpdateEntityAsync(toUpdate);
            
            return updated.Adapt<ResourceDto>();
        }

        public Task DeleteResource(int id)
        {
            throw new NotImplementedException();
        }
    }
}
