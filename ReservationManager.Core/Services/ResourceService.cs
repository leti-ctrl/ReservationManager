using Mapster;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Exceptions;
using ReservationManager.Core.Interfaces;
using ReservationManager.DomainModel.Operation;
using ReservationManager.Persistence.Interfaces;

namespace ReservationManager.Core.Services
{
    public class ResourceService : IResourceService
    {
        private readonly IResourceRepository _resourceRepository;
        private readonly IResourceTypeRepository _resourceTypeRepository;

        public ResourceService(IResourceRepository resourceRepository, IResourceTypeRepository resourceTypeRepository)
        {
            _resourceRepository = resourceRepository;
            _resourceTypeRepository = resourceTypeRepository;
        }

        public async Task<IEnumerable<ResourceDto>> GetAllResources()
        {
            var resources = await _resourceRepository.GetAllEntitiesAsync();

            if(resources == null) 
                return Enumerable.Empty<ResourceDto>();
            return resources.Select(x => x.Adapt<ResourceDto>());
        }

        public async Task<ResourceDto> GetResourceById(int resourceId)
        {
            var resource = await _resourceRepository.GetEntityByIdAsync(resourceId) 
                ?? throw new EntityNotFoundException($"Resource id {resourceId} not found");

            return resource.Adapt<ResourceDto>();
        }

        public Task<IEnumerable<ResourceDto>> GetFilteredResources(FilterDto filters)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ResourceDto>> GetResourcesAvailability(FilterDto filters)
        {
            throw new NotImplementedException();
        }

        public async Task<ResourceDto> CreateResource(UpsertResourceDto resource)
        {
            var type = await _resourceTypeRepository.GetTypeByCode(resource.Type) 
                ?? throw new InvalidCodeTypeException($"Resource type {resource.Type} not valid");

            var resourceToAdd = new Resource()
            {
                Description = resource.Description,
                Type = type,
                TypeId = type.Id,
            };

            var added = await _resourceRepository.CreateEntityAsync(resourceToAdd);
            return added.Adapt<ResourceDto>();
        }

        public Task<ResourceDto> UpdateResource(UpsertResourceDto resource)
        {
            throw new NotImplementedException();
        }

        public Task DeleteResource(int id)
        {
            throw new NotImplementedException();
        }
    }
}
