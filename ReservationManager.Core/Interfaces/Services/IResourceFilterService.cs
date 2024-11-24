using ReservationManager.Core.Dtos;

namespace ReservationManager.Core.Interfaces.Services;

public interface IResourceFilterService
{
    Task<IEnumerable<ResourceDto>> GetFilteredResources(ResourceFilterDto resourceFilters);
}