using ReservationManager.Core.Dtos;
using ReservationManager.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

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
