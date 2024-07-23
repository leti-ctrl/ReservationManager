using ReservationManager.DomainModel.Operation;
using ReservationManager.Persistence.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Persistence.Interfaces
{
    public interface IUserRepository : ICrudRepository<User>
    {
        User GetByEmail(string email);
        IEnumerable<User> GetByName(string name, string surname);
        IEnumerable<User> GetByType(string code);
    }
}
