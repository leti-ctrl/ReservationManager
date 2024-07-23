using ReservationManager.DomainModel.Meta;
using ReservationManager.Persistence.Interfaces;
using ReservationManager.Persistence.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Persistence.Repositories
{
    public class UserTypeRepository : TypeBaseRepository<UserType>, IUserTypeRepository
    {
        public UserTypeRepository(ReservationManagerDbContext dbContext) : base(dbContext)
        {
        }
    }
}
