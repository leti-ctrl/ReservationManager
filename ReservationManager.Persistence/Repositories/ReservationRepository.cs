using Microsoft.EntityFrameworkCore;
using ReservationManager.DomainModel.Operation;
using ReservationManager.Persistence.Interfaces;
using ReservationManager.Persistence.Interfaces.Base;
using ReservationManager.Persistence.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Persistence.Repositories
{
    public class ReservationRepository : CrudBaseRepository<Reservation>, IReservationRepository
    {
        public ReservationRepository(ReservationManagerDbContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<Reservation> GetByDay(DateOnly day)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Reservation> GetByResource(int resourceId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Reservation> GetByType(string code)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Reservation> GetByUser(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
