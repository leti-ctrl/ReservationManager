using ReservationManager.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Core.Interfaces
{
    public interface IReservationTypeService
    {
        IEnumerable<ReservationTypeDto> GetAllReservationType();
        ReservationTypeDto CreateReservationType(string code, TimeOnly start, TimeOnly end);
        ReservationTypeDto UpdateReservationType(int id, ReservationTypeDto type);
        void DeleteReservationType(int id);
    }
}
