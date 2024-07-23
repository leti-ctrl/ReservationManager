using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReservationManager.DomainModel.Base;
using ReservationManager.Persistence.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Persistence.Repositories.Base
{
    public class TypeBaseRepository<T> : RepositoryBase<T>, ITypeRepository<T>
        where T : BaseType
    {
        public TypeBaseRepository(ReservationManagerDbContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<T> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
