using ReservationManager.DomainModel.Daos;

namespace ReservationManager.Core.Interfaces.Repositories;

public interface ILazyVsEagerRepository
{
    Task<IEnumerable<ResourceRepoDao>> EagerGetAllBusyResourcesFromTodayAsync();
    Task<IEnumerable<ResourceRepoDao>> LazyGetAllBusyResourcesFromTodayAsync();
    
    Task<IEnumerable<ResourceRepoDao>> EagerGetBusyResourceByResourceIdAndDayAsync(int resourceId, DateOnly day);
    Task<IEnumerable<ResourceRepoDao>> LazyGetBusyResourceByResourceIdAndDayAsync(int resourceId, DateOnly day);
    
    Task<IEnumerable<ResourceRepoDao>> EagerGetBusyResourceByResourceTypeIdAndDayAsync(int resourceTypeId, DateOnly day);
    Task<IEnumerable<ResourceRepoDao>> LazyGetBusyResourceByResourceTypeIdAndDayAsync(int resourceTypeId, DateOnly day);
}
