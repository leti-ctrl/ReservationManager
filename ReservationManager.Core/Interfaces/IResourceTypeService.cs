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
        Task<IEnumerable<ResourceTypeDto>> GetAllResourceTypes();
        Task<ResourceTypeDto> CreateResourceType(string code);
        Task<ResourceTypeDto> UpdateResourceType(int id, string code);
        Task DeleteResourceType(int id);
    }
}
