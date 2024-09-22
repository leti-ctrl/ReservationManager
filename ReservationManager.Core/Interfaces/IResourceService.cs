using ReservationManager.Core.Dtos;

namespace ReservationManager.Core.Interfaces
{
    public interface IResourceService
    {
        IEnumerable<ResourceDto> GetResources(FilterDto filters);
        IEnumerable<ResourceDto> GetResourcesAvailability(FilterDto filters);
        ResourceDto CreateResource(UpsertResourceDto resource);
        ResourceDto UpdateResource(UpsertResourceDto resource);
        void DeleteResource(int id);
    }
}
