using Mapster;
using ReservationManager.Core.Dtos;
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
            var oldResourceType = await _resourceTypeRepository.GetTypeById(id);
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
            await _resourceTypeRepository.DeleteTypeAsync(id);
        }
    }
}
