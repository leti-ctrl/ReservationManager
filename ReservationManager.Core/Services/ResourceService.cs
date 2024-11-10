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
        private readonly IResourceValidator _resourceValidator;

        public ResourceService(IResourceRepository resourceRepository, IResourceValidator resourceValidator)
        {
            _resourceRepository = resourceRepository;
            _resourceValidator = resourceValidator;
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

        public Task<IEnumerable<ResourceDto>> GetFilteredResources(FilterDto filters)
        {
            throw new NotImplementedException();
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
