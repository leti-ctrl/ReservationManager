using Mapster;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Exceptions;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.Core.Interfaces.Services;
using ReservationManager.DomainModel.Meta;

namespace ReservationManager.Core.Services
{
    public class ResourceTypeService : IResourceTypeService
    {
        private readonly IResourceTypeRepository _resourceTypeRepository;
        private readonly IResourceService _resourceService;

        public ResourceTypeService(IResourceTypeRepository resourceTypeRepository, IResourceService resourceService)
        {
            _resourceTypeRepository = resourceTypeRepository;
            _resourceService = resourceService;
        }
        public async Task<IEnumerable<ResourceTypeDto>> GetAllResourceTypes()
        {
            var resourceTypes = await _resourceTypeRepository.GetAllTypesAsync();
            return resourceTypes.Select(rt => rt.Adapt<ResourceTypeDto>());
        }

        public async Task<ResourceTypeDto?> UpdateResourceType(int id, UpsertResourceTypeDto resource)
        {
            var oldResourceType = await _resourceTypeRepository.GetTypeById(id);
            if (oldResourceType == null)
                return null;
            
            oldResourceType.Code = resource.Code;
            oldResourceType.Name = resource.Name;

            var updated = await _resourceTypeRepository.UpdateTypeAsync(oldResourceType);
            return updated.Adapt<ResourceTypeDto>();
        }

        public async Task<ResourceTypeDto> CreateResourceType(UpsertResourceTypeDto resource)
        { 
            var created = await _resourceTypeRepository.CreateTypeAsync(resource.Adapt<ResourceType>());
            return created.Adapt<ResourceTypeDto>();
        }

        public async Task DeleteResourceType(int id)
        {
            var toDelete = await _resourceTypeRepository.GetTypeById(id)
                ?? throw new EntityNotFoundException($"Resource Type with id {id} not found");

            var exists = await _resourceService.GetFilteredResources(new ResourceFilterDto() { TypeId = id });
            if (exists.Any())
                throw new DeleteNotPermittedException($"Cannot delete {toDelete.Code} because exits resources with this type");

            await _resourceTypeRepository.DeleteTypeAsync(toDelete);
        }
    }
}
