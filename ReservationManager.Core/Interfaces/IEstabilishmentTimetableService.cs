using ReservationManager.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Core.Interfaces
{
    public interface IEstabilishmentTimetableService
    {
        Task<IEnumerable<EstabilishmentTimetableDto>> GetAll();
        Task<IEnumerable<EstabilishmentTimetableDto>> GetByTypeId(int typeId);
        Task<EstabilishmentTimetableDto> Create(UpsertEstabilishmentTimetableDto entity);
    }
}
