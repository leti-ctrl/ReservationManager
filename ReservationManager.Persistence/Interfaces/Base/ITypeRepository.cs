using ReservationManager.DomainModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Persistence.Interfaces.Base
{
    public interface ITypeRepository<T> where T : BaseType
    {
        IEnumerable<T> GetAll();
    }
}
