using ReservationManager.Core.Dtos;
using ReservationManager.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Core.Services
{
    public class ReservationTypeService : IReservationTypeService
    {
        public ReservationTypeDto CreateReservationType(string code, TimeOnly start, TimeOnly end)
        {
            throw new NotImplementedException();
        }

        public void DeleteReservationType(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ReservationTypeDto> GetAllReservationType()
        {
            throw new NotImplementedException();
        }

        public ReservationTypeDto UpdateReservationType(int id, ReservationTypeDto type)
        {
            throw new NotImplementedException();
        }
    }
}
