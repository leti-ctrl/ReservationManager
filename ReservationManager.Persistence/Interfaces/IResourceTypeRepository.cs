using ReservationManager.DomainModel.Meta;
using ReservationManager.Persistence.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Persistence.Interfaces
{
    public interface IResourceTypeRepository : ICrudTypeRepository<ResourceType>
    {
    }
}
