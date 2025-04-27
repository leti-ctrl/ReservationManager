using ReservationManager.DomainModel.Dtos;

namespace ReservationManager.Core.Interfaces.Repositories;

public interface ILazyVsEagerRepository
{
    Task<IEnumerable<ResourceRepoDto>> EagerGetAllResourcesAsDtoAsync();
    Task<IEnumerable<ResourceRepoDto>> LazyGetAllResourcesAsDtoAsync();
}