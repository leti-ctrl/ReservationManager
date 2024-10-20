﻿using ReservationManager.DomainModel.Meta;
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
    public class ResourceTypeRepository : CrudTypeBaseRepository<ResourceType>, IResourceTypeRepository
    {
        public ResourceTypeRepository(ReservationManagerDbContext dbContext) : base(dbContext)
        {
        }
    }
}
