using ReservationManager.Core.Dtos;
using ReservationManager.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Core.Services
{
    public class ReservationService : IReservationService
    {
        public ReservationDto CreateReservation(UpsertReservationDto reservation)
        {
            throw new NotImplementedException();
        }

        public void DeleteReservation(int id)
        {
            throw new NotImplementedException();
        }

        public ReservationDto GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ReservationDto> GetUserReservation(int userId)
        {
            throw new NotImplementedException();
        }

        public ReservationDto UpdateReservation(UpsertReservationDto reservation)
        {
            throw new NotImplementedException();
        }
    }
}
