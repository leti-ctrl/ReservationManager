using ReservationManager.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Core.Interfaces
{
    public interface ITimetableTypeService
    {
        Task<IEnumerable<TimetableTypeDto>> GetAllTypes();
        Task<TimetableTypeDto> GetById(int id);
    }
}
