using ReservationManager.Core.Dtos;

namespace ReservationManager.Core.Interfaces.Services
{
    public interface IResourceTypeService
    {
        Task<IEnumerable<ResourceTypeDto>> GetAllResourceTypes();
        Task<ResourceTypeDto> CreateResourceType(UpsertResourceTypeDto resource);
        Task<ResourceTypeDto?> UpdateResourceType(int id, UpsertResourceTypeDto resource);
        Task DeleteResourceType(int id);
    }
}
