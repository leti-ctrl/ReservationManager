﻿using ReservationManager.DomainModel.Meta;
using ReservationManager.Persistence.Interfaces;
using ReservationManager.Persistence.Repositories.Base;

namespace ReservationManager.Persistence.Repositories
{
    public class UserTypeRepository : CrudTypeBaseRepository<UserType>, IUserTypeRepository
    {
        public UserTypeRepository(ReservationManagerDbContext dbContext) : base(dbContext)
        {
        }
    }
}
