using Mapster;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Exceptions;
using ReservationManager.Core.Interfaces;
using ReservationManager.Persistence.Interfaces;

namespace ReservationManager.Core.Services
{
    public class ResourceTypeService : IResourceTypeService
    {
        private readonly IResourceTypeRepository _resourceTypeRepository;

        public ResourceTypeService(IResourceTypeRepository resourceTypeRepository)
        {
            _resourceTypeRepository = resourceTypeRepository;
        }
        public async Task<IEnumerable<ResourceTypeDto>> GetAllResourceTypes()
        {
            var resourceTypes = await _resourceTypeRepository.GetAllTypesAsync();
            if (resourceTypes == null)
                return Enumerable.Empty<ResourceTypeDto>();

            return resourceTypes.Select(rt => rt.Adapt<ResourceTypeDto>());
        }

        public async Task<ResourceTypeDto> UpdateResourceType(int id, string code)
        {
            var oldResourceType = await _resourceTypeRepository.GetTypeById(id)
                ?? throw new EntityNotFoundException($"Resource Type with id {id} not found");
            oldResourceType.Code = code;

            var updated = await _resourceTypeRepository.UpdateTypeAsync(oldResourceType);
            return updated.Adapt<ResourceTypeDto>();
        }

        public async Task<ResourceTypeDto> CreateResourceType(string code)
        {
            var newResouceType = await _resourceTypeRepository.CreateTypeAsync(new DomainModel.Meta.ResourceType() { Code = code });
            return newResouceType.Adapt<ResourceTypeDto>();
        }

        public async Task DeleteResourceType(int id)
        {
            var toDelete = await _resourceTypeRepository.GetTypeById(id)
                ?? throw new EntityNotFoundException($"Resource Type with id {id} not found");

            await _resourceTypeRepository.DeleteTypeAsync(toDelete);
        }
    }
}
