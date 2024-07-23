using ReservationManager.DomainModel.Operation;
using ReservationManager.Persistence.Interfaces;
using ReservationManager.Persistence.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Persistence.Repositories
{
    public class UserRepository : CrudBaseRepository<User>, IUserRepository
    {
        public UserRepository(ReservationManagerDbContext dbContext) : base(dbContext)
        {
        }

        public User GetByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetByName(string name, string surname)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetByType(string code)
        {
            throw new NotImplementedException();
        }
    }
}
