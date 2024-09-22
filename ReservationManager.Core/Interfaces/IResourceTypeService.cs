using ReservationManager.Core.Dtos;

namespace ReservationManager.Core.Interfaces
{
    public interface IResourceTypeService
    {
        Task<IEnumerable<ResourceTypeDto>> GetAllResourceTypes();
        Task<ResourceTypeDto> CreateResourceType(string code);
        Task<ResourceTypeDto> UpdateResourceType(int id, string code);
        Task DeleteResourceType(int id);
    }
}
