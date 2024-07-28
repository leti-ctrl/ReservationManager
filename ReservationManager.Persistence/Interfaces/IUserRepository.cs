using ReservationManager.DomainModel.Operation;
using ReservationManager.Persistence.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Persistence.Interfaces
{
    public interface IUserRepository : ICrudEntityRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetByTypeAsync(string code);
        Task<IEnumerable<User>> GetByNameAsync(string name, string surname);
    }
}
