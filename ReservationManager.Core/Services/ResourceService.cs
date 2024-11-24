using Mapster;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Exceptions;
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
        private readonly IResourceFilterService _resourceFilterService;
        
        private readonly IReservationRepository _reservationRepository;

        public ResourceService(IResourceRepository resourceRepository, IResourceValidator resourceValidator,
            IReservationRepository reservationRepository, IResourceFilterService resourceFilterService)
        {
            _resourceRepository = resourceRepository;
            _resourceValidator = resourceValidator;
            _reservationRepository = reservationRepository;
            _resourceFilterService = resourceFilterService;
        }

        public async Task<IEnumerable<ResourceDto>> GetAllResources()
        {
            var resources = (await _resourceRepository.GetAllEntitiesAsync()).ToList();

            return !resources.Any()
                ? Enumerable.Empty<ResourceDto>()
                : resources.Select(x => x.Adapt<ResourceDto>()).OrderBy(r => r.Type.Code);
        }

        public async Task<IEnumerable<ResourceDto>> GetFilteredResources(ResourceFilterDto resourceFilters)
        {
            return await _resourceFilterService.GetFilteredResources(resourceFilters);
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