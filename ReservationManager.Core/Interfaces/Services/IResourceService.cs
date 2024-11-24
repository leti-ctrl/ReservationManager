using ReservationManager.Core.Dtos;

namespace ReservationManager.Core.Interfaces.Services
{
    public interface IResourceService
    {
        Task<IEnumerable<ResourceDto>> GetAllResources();
        Task<IEnumerable<ResourceDto>> GetFilteredResources(ResourceFilterDto resourceFilters);
        Task<ResourceDto> CreateResource(UpsertResourceDto resource);
        Task<ResourceDto?> UpdateResource( int id, UpsertResourceDto resource);
        Task DeleteResource(int id);
    }
}
