using ReservationManager.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Core.Interfaces
{
    public interface IResourceTypeService
    {
        IEnumerable<ResourceTypeDto> GetAllResourceTypes();
        ResourceTypeDto CreateResourceType(string code);
        ResourceTypeDto UpdateResourceType(int id, string code);
        void DeleteResourceType(int id);
    }
}
