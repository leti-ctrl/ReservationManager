using ReservationManager.Core.Dtos;

namespace ReservationManager.Core.Interfaces
{
    public interface IResourceService
    {
        Task<IEnumerable<ResourceDto>> GetAllResources();
        Task<ResourceDto> GetResourceById(int resourceId);
        Task<IEnumerable<ResourceDto>> GetFilteredResources(FilterDto filters);
        Task<ResourceDto> CreateResource(UpsertResourceDto resource);
        Task<ResourceDto> UpdateResource( int id, UpsertResourceDto resource);
        Task DeleteResource(int id);
    }
}
