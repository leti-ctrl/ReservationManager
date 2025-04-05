using Mapster;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Exceptions;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.Core.Interfaces.Services;
using ReservationManager.Core.Interfaces.Validators;
using ReservationManager.DomainModel.Operation;

namespace ReservationManager.Core.Services
{
    public class ResourceService(
        IResourceRepository resourceRepository,
        IResourceValidator resourceValidator,
        IReservationRepository reservationRepository,
        IResourceFilterService resourceFilterService)
        : IResourceService
    {
        public async Task<IEnumerable<ResourceDto>> GetAllResources()
        {
            var resources = (await resourceRepository.GetAllEntitiesAsync()).ToList();

            return !resources.Any()
                ? Enumerable.Empty<ResourceDto>()
                : resources.Select(x => x.Adapt<ResourceDto>()).OrderBy(r => r.Type.Code);
        }

        public async Task<IEnumerable<ResourceDto>> GetFilteredResources(ResourceFilterDto resourceFilters)
        {
            return await resourceFilterService.GetFilteredResources(resourceFilters);
        }

        public async Task<ResourceDto> CreateResource(UpsertResourceDto resource)
        {
            var existType = await resourceValidator.ValidateResourceType(resource.TypeId);
            if (!existType)
                throw new InvalidCodeTypeException($"Resource type {resource.TypeId} is not valid");

            var added = await resourceRepository.CreateEntityAsync(resource.Adapt<Resource>());
            return added.Adapt<ResourceDto>();
        }

        public async Task<ResourceDto?> UpdateResource(int id, UpsertResourceDto resource)
        {
            var validateModel = await resourceValidator.ValidateResourceType(resource.TypeId) 
                                    && await resourceValidator.ExistingResouceId(id);
            if (!validateModel)
                return null;

            var toUpdate = resource.Adapt<Resource>();
            toUpdate.Id = id;

            var updated = await resourceRepository.UpdateEntityAsync(toUpdate);

            return updated.Adapt<ResourceDto>();
        }

        public async Task DeleteResource(int id)
        {
            var entity = await resourceRepository.GetEntityByIdAsync(id) 
                         ?? throw new EntityNotFoundException($"Resource with id {id} not found");
            
            var reservations = await reservationRepository.GetReservationByResourceIdAfterTodayAsync(id);
            if(reservations.Any())
                throw new DeleteNotPermittedException("Resource cannot be deleted because of existing reservation");
            
            await resourceRepository.DeleteEntityAsync(entity);
        }
    }
}