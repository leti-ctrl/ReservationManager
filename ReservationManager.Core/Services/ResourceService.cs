using ReservationManager.Core.Dtos;
using ReservationManager.Core.Interfaces;

namespace ReservationManager.Core.Services
{
    public class ResourceService : IResourceService
    {
        public ResourceDto CreateResource(UpsertResourceDto resource)
        {
            throw new NotImplementedException();
        }

        public void DeleteResource(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ResourceDto> GetResources(FilterDto filters)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ResourceDto> GetResourcesAvailability(FilterDto filters)
        {
            throw new NotImplementedException();
        }

        public ResourceDto UpdateResource(UpsertResourceDto resource)
        {
            throw new NotImplementedException();
        }
    }
}
