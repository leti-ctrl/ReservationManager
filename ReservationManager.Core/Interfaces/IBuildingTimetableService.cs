using ReservationManager.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Core.Interfaces
{
    public interface IBuildingTimetableService
    {
        Task<IEnumerable<BuildingTimetableDto>> GetAll();
        Task<IEnumerable<BuildingTimetableDto>> GetByTypeId(int typeId);
        Task<BuildingTimetableDto> Create(UpsertEstabilishmentTimetableDto entity);
        Task<BuildingTimetableDto> Update(int id, UpsertEstabilishmentTimetableDto entity);
    }
}
