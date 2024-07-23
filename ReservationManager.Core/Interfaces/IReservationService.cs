using ReservationManager.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Core.Interfaces
{
    public interface IReservationService
    {
        IEnumerable<ReservationDto> GetUserReservation(int userId);
        ReservationDto GetById(int id);
        ReservationDto CreateReservation(UpsertReservationDto reservation);
        ReservationDto UpdateReservation(UpsertReservationDto reservation);
        void DeleteReservation(int id);
       
    }
}
